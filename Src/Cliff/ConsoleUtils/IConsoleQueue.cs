using System.Threading.Tasks;

namespace Cliff.ConsoleUtils
{
	public interface IConsoleQueue
	{
		public Task EnqueueOutputAsync(string value);
		Task EnqueueBreakLine(int? length = null, char symbol = '-', bool newLineBefore = true, bool newLineAfter = true);
	}
}
