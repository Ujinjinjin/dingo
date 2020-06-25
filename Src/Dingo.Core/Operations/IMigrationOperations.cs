using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	/// <summary> Operations with database migration files </summary>
	public interface IMigrationOperations
	{
		/// <summary> Run migrations </summary>
		/// <param name="projectMigrationsRootPath">Root path where all project migrations are stored</param>
		Task RunMigrationsAsync(string projectMigrationsRootPath);
	}
}