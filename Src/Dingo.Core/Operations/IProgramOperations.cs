using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	public interface IProgramOperations
	{
		Task RunMigrationsAsync(string[] args);
	}
}