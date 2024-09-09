using System.Data;
using Dapper;
using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Dingo.Core.Repository.Command;
using Dingo.Core.Repository.Models;
using Microsoft.Extensions.Logging;
using Npgsql;
using Trico.Configuration;

namespace Dingo.Core.Repository;

internal class DatabaseRepository : IRepository
{
	private readonly IConnectionResolverFactory _connectionResolverFactory;
	private readonly ICommandProviderFactory _commandProviderFactory;
	private readonly IConfiguration _configuration;
	private readonly ILogger _logger;

	public DatabaseRepository(
		IConnectionResolverFactory connectionResolverFactory,
		ICommandProviderFactory commandProviderFactory,
		IConfiguration configuration,
		ILoggerFactory loggerFactory
	)
	{
		_connectionResolverFactory = connectionResolverFactory.Required(nameof(connectionResolverFactory));
		_commandProviderFactory = commandProviderFactory.Required(nameof(commandProviderFactory));
		_configuration = configuration.Required(nameof(configuration));
		_logger = loggerFactory.Required(nameof(loggerFactory))
			.CreateLogger<DatabaseRepository>()
			.Required(nameof(loggerFactory));

		DefaultTypeMap.MatchNamesWithUnderscores = true;
	}

	public async Task<bool> TryHandshakeAsync(CancellationToken ct = default)
	{
		try
		{
			await HandshakeAsync(ct);
			return true;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Can't establish database connection");
			return false;
		}
	}

	private async Task HandshakeAsync(CancellationToken ct = default)
	{
		await using var resolver = _connectionResolverFactory.Create();
		if (resolver.Connection.State == ConnectionState.Open)
		{
			return;
		}

		await resolver.Connection.OpenAsync(ct);
	}

	public async Task<bool> SchemaExistsAsync(
		string schema,
		CancellationToken ct = default
	)
	{
		await using var resolver = _connectionResolverFactory.Create();
		var command = _commandProviderFactory.Create()
			.SelectSchema(schema);

		var result = await resolver.Connection.QueryAsync<string>(command);

		return result.FirstOrDefault() != null;
	}

	public async Task<bool> IsDatabaseEmptyAsync(CancellationToken ct = default)
	{
		var dingoSchemaName = _configuration.Get(Configuration.Key.SchemaName);
		return !await SchemaExistsAsync(dingoSchemaName, ct);
	}

	public async Task<IReadOnlyList<MigrationComparisonOutput>> GetMigrationsComparisonAsync(
		IReadOnlyList<Migration> migrations,
		CancellationToken ct = default
	)
	{
		await using var resolver = _connectionResolverFactory.Create();
		var migrationInfoInputs = migrations.Select(ToMigrationsInfoInput).ToArray();

		var command = _commandProviderFactory.Create()
			.GetMigrationsStatus(migrationInfoInputs);

		var result = await resolver.Connection.QueryAsync<MigrationComparisonOutput>(command);
		return result.ToArray();
	}

	// TODO: extract
	private MigrationComparisonInput ToMigrationsInfoInput(Migration migration)
	{
		return new MigrationComparisonInput(migration.Hash.Value, migration.Path.Relative);
	}

	public async Task<int> GetNextPatchAsync(CancellationToken ct = default)
	{
		await using var resolver = _connectionResolverFactory.Create();
		var command = _commandProviderFactory.Create()
			.GetNextPatch();

		var result = await resolver.Connection.QueryAsync<int>(command);
		return result.Single();
	}

	public async Task<IReadOnlyList<PatchMigration>> GetLastPatchMigrationsAsync(
		int patchCount,
		CancellationToken ct = default
	)
	{
		await using var resolver = _connectionResolverFactory.Create();
		var command = _commandProviderFactory.Create()
			.GetLastPatchMigrations(patchCount);

		var result = await resolver.Connection.QueryAsync<PatchMigration>(command);
		return result.ToArray();
	}

	public async Task RegisterMigrationAsync(
		Migration migration,
		int patchNumber,
		CancellationToken ct = default
	)
	{
		await using var resolver = _connectionResolverFactory.Create();
		var command = _commandProviderFactory.Create()
			.RegisterMigration(migration, patchNumber);

		await resolver.Connection.ExecuteAsync(command);
	}

	public async Task RevertPatchAsync(int patchNumber, CancellationToken ct = default)
	{
		await using var resolver = _connectionResolverFactory.Create();
		var command = _commandProviderFactory.Create()
			.RevertPatch(patchNumber);

		await resolver.Connection.ExecuteAsync(command);
	}

	public async Task CompletePatchAsync(int patchNumber, CancellationToken ct = default)
	{
		await using var resolver = _connectionResolverFactory.Create();
		var command = _commandProviderFactory.Create()
			.CompletePatch(patchNumber);

		await resolver.Connection.ExecuteAsync(command);
	}

	public async Task ExecuteAsync(string sql, CancellationToken ct = default)
	{
		await using var resolver = _connectionResolverFactory.Create();
		await resolver.Connection.ExecuteAsync(sql, commandType: CommandType.Text);
	}

	public async Task ReloadTypesAsync(CancellationToken ct = default)
	{
		await using var resolver = _connectionResolverFactory.Create();
		if (resolver.Connection is NpgsqlConnection npgsqlConnection)
		{
			await resolver.Connection.OpenAsync(ct);
			await npgsqlConnection.ReloadTypesAsync();
		}
	}
}
