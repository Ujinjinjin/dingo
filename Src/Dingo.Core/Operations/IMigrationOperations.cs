using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	/// <summary> Operations with database migration files </summary>
	public interface IMigrationOperations
	{
		/// <summary> Perform handshake connection to database to validate connection string </summary>
		/// <param name="configPath">Custom path to configuration file</param>
		Task HandshakeDatabaseConnectionAsync(string configPath = null);

		/// <summary> Run migrations </summary>
		/// <param name="migrationsRootPath">Root path where all project migrations are stored</param>
		/// <param name="configPath">Custom path to configuration file</param>
		/// <param name="silent">Show less info about migration status</param>
		/// <param name="connectionString">Database connection string</param>
		/// <param name="providerName">Database provider name</param>
		/// <param name="migrationSchema">Database schema for you migrations</param>
		/// <param name="migrationTable">Database table, where all migrations are stored</param>
		Task RunMigrationsAsync(
			string migrationsRootPath,
			string configPath = null,
			bool silent = false,
			string connectionString = null,
			string providerName = null,
			string migrationSchema = null,
			string migrationTable = null
		);

		/// <summary> Show status of migrations </summary>
		/// <param name="migrationsRootPath">Root path where all project migrations are stored</param>
		/// <param name="configPath">Custom path to configuration file</param>
		/// <param name="silent">Show less info about migration status</param>
		Task ShowMigrationsStatusAsync(string migrationsRootPath, string configPath = null, bool silent = false);
	}
}