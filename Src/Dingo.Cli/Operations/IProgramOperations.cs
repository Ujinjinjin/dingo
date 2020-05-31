using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	public interface IProgramOperations
	{
		Task RunAsync(string[] args);
	}
}