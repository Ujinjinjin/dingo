using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	public interface IMigrationOperations
	{
		Task RunMigrationsAsync(string projectMigrationsRootPath);
	}
}