using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	public interface IProgramOperations
	{
		Task RunMigrationsAsync(string[] args);
	}
}