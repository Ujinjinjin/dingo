using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	/// <summary> Operations with database migration files </summary>
	public interface IMigrationOperations
	{
		/// <summary> Run migrations </summary>
		/// <param name="migrationsRootPath">Root path where all project migrations are stored</param>
		/// <param name="configPath">Custom path to configuration file</param>
		/// <param name="silent">Show less info about migration status</param>
		Task RunMigrationsAsync(string migrationsRootPath, string configPath = null, bool silent = false);

		/// <summary> Show status of migrations </summary>
		/// <param name="migrationsRootPath">Root path where all project migrations are stored</param>
		/// <param name="configPath">Custom path to configuration file</param>
		/// <param name="silent">Show less info about migration status</param>
		Task ShowMigrationsStatusAsync(string migrationsRootPath, string configPath = null, bool silent = false);
	}
}