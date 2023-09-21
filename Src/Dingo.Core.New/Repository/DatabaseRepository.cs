using System.Data;
using Dapper;
using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Dingo.Core.Repository.Command;
using Dingo.Core.Repository.Models;
using Microsoft.Extensions.Logging;

namespace Dingo.Core.Repository;

internal class DatabaseRepository : IRepository
{
	private readonly IConnectionFactory _connectionFactory;
	private readonly ICommandProvider _commandProvider;
	private readonly ILogger _logger;


	public DatabaseRepository(
		IConnectionFactory connectionFactory,
		ICommandProviderFactory commandProviderFactory,
		ILoggerFactory loggerFactory
	)
	{
		_connectionFactory = connectionFactory.Required(nameof(connectionFactory));
		_commandProvider = commandProviderFactory.Required(nameof(commandProviderFactory))
			.Create();
		_logger = loggerFactory.Required(nameof(loggerFactory))
			.CreateLogger<DatabaseRepository>()
			.Required(nameof(loggerFactory));

		DefaultTypeMap.MatchNamesWithUnderscores = true;
	}

	public bool TryHandshake()
	{
		try
		{
			Handshake();
			return true;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Can't establish database connection");
			return false;
		}
	}

	private void Handshake()
	{
		using var connection = _connectionFactory.Create();

		if (connection.State == ConnectionState.Open)
		{
			return;
		}

		connection.Open();
	}

	public async Task<bool> SchemaExistsAsync(string schema, CancellationToken ct = default)
	{
		using var connection = _connectionFactory.Create();
		var command = _commandProvider.SelectSchema(schema);

		var result = await connection.QueryAsync<string>(command);

		return result.FirstOrDefault() != null;
	}

	public async Task<IReadOnlyList<MigrationComparisonOutput>> GetMigrationsComparisonAsync(
		IReadOnlyList<Migration> migrations,
		CancellationToken ct = default
	)
	{
		var migrationInfoInputs = migrations.Select(ToMigrationsInfoInput).ToArray();

		using var connection = _connectionFactory.Create();
		var command = _commandProvider.GetMigrationsStatus(migrationInfoInputs);

		var result = await connection.QueryAsync<MigrationComparisonOutput>(command);
		return result.ToArray();
	}

	private MigrationComparisonInput ToMigrationsInfoInput(Migration migration)
	{
		return new MigrationComparisonInput(migration.Hash.Value, migration.Path.Relative);
	}
}
