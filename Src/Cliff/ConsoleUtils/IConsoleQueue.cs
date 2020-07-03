using System.Threading.Tasks;

namespace Cliff.ConsoleUtils
{
	public interface IConsoleQueue
	{
		public Task EnqueueOutputAsync(string value);
		Task EnqueueStartBlockLine(int? length = null);
		Task EnqueueEndBlockLine(int? length = null);
	}
}
