namespace Dingo.Core.Services;

/// <summary> Operations with database migration files </summary>
public interface IMigrationService
{
	/// <summary> Create new migration file with specified name </summary>
	/// <param name="name">Migration name</param>
	/// <param name="path">Migration path</param>
	Task CreateMigrationFileAsync(string name, string path);
		
	/// <summary> Perform handshake connection to database to validate connection string </summary>
	/// <param name="configPath">Custom path to configuration file</param>
	Task HandshakeDatabaseConnectionAsync(string configPath = null);

	/// <summary> Run migrations </summary>
	/// <param name="migrationsDir">Root path where all project migrations are stored</param>
	/// <param name="configPath">Custom path to configuration file</param>
	/// <param name="silent">Show less info about migration status</param>
	/// <param name="connectionString">Database connection string</param>
	/// <param name="providerName">Database provider name</param>
	/// <param name="migrationSchema">Database schema for you migrations</param>
	/// <param name="migrationTable">Database table, where all migrations are stored</param>
	Task RunMigrationsAsync(
		string migrationsDir,
		string configPath = null,
		bool silent = false,
		string connectionString = null,
		string providerName = null,
		string migrationSchema = null,
		string migrationTable = null
	);

	/// <summary> Show status of migrations </summary>
	/// <param name="migrationsDir">Root path where all project migrations are stored</param>
	/// <param name="configPath">Custom path to configuration file</param>
	/// <param name="silent">Show less info about migration status</param>
	Task ShowMigrationsStatusAsync(string migrationsDir, string configPath = null, bool silent = false);
}