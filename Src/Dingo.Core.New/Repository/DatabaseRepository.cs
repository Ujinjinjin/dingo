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
	private readonly IConnectionFactory _connectionFactory;
	private readonly ICommandProvider _commandProvider;
	private readonly IConfiguration _configuration;
	private readonly ILogger _logger;

	public DatabaseRepository(
		IConnectionFactory connectionFactory,
		ICommandProviderFactory commandProviderFactory,
		IConfiguration configuration,
		ILoggerFactory loggerFactory
	)
	{
		_connectionFactory = connectionFactory.Required(nameof(connectionFactory));
		_commandProvider = commandProviderFactory.Required(nameof(commandProviderFactory)).Create();
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
		await using var connection = _connectionFactory.Create();

		if (connection.State == ConnectionState.Open)
		{
			return;
		}

		await connection.OpenAsync(ct);
	}

	public async Task<bool> SchemaExistsAsync(string schema, CancellationToken ct = default)
	{
		await using var connection = _connectionFactory.Create();
		var command = _commandProvider.SelectSchema(schema);

		var result = await connection.QueryAsync<string>(command);

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
		var migrationInfoInputs = migrations.Select(ToMigrationsInfoInput).ToArray();

		await using var connection = _connectionFactory.Create();
		var command = _commandProvider.GetMigrationsStatus(migrationInfoInputs);

		var result = await connection.QueryAsync<MigrationComparisonOutput>(command);
		return result.ToArray();
	}

	// TODO: extract
	private MigrationComparisonInput ToMigrationsInfoInput(Migration migration)
	{
		return new MigrationComparisonInput(migration.Hash.Value, migration.Path.Relative);
	}

	public async Task<int> GetNextPatchAsync(CancellationToken ct = default)
	{
		await using var connection = _connectionFactory.Create();
		var command = _commandProvider.GetNextPatch();

		var result = await connection.QueryAsync<int>(command);
		return result.Single();
	}

	public async Task<IReadOnlyList<PatchMigration>> GetLastPatchMigrationsAsync(
		int patchCount,
		CancellationToken ct = default
	)
	{
		await using var connection = _connectionFactory.Create();
		var command = _commandProvider.GetLastPatchMigrations(patchCount);

		var result = await connection.QueryAsync<PatchMigration>(command);
		return result.ToArray();
	}

	public async Task RegisterMigrationAsync(Migration migration, int patchNumber, CancellationToken ct = default)
	{
		await using var connection = _connectionFactory.Create();
		var command = _commandProvider.RegisterMigration(migration, patchNumber);

		await connection.ExecuteAsync(command);
	}

	public async Task RevertPatchAsync(int patchNumber, CancellationToken ct = default)
	{
		await using var connection = _connectionFactory.Create();
		var command = _commandProvider.RevertPatch(patchNumber);

		await connection.ExecuteAsync(command);
	}

	public async Task CompletePatchAsync(int patchNumber, CancellationToken ct = default)
	{
		await using var connection = _connectionFactory.Create();
		var command = _commandProvider.CompletePatch(patchNumber);

		await connection.ExecuteAsync(command);
	}

	public async Task ExecuteAsync(string sql, CancellationToken ct = default)
	{
		await using var connection = _connectionFactory.Create();
		await connection.ExecuteAsync(sql, commandType: CommandType.Text);
	}

	public async Task ReloadTypesAsync(CancellationToken ct = default)
	{
		await using var connection = _connectionFactory.Create();
		if (connection is NpgsqlConnection npgsqlConnection)
		{
			await connection.OpenAsync(ct);
			await npgsqlConnection.ReloadTypesAsync();
		}
	}
}
