using System.Threading.Tasks;

namespace Cliff.ConsoleUtils
{
	public interface IConsoleQueue
	{
		public Task EnqueueOutputAsync(string value);
	}
}
