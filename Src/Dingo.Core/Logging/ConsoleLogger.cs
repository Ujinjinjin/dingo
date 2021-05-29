using Dingo.Core.Config;
using Dingo.Core.Factories;

namespace Dingo.Core.Logging
{
	/// <summary> Logger with output to file </summary>
	internal sealed class ConsoleLogger : LoggerBase
	{
		public ConsoleLogger(
			string categoryName,
			IConfigWrapper configWrapper,
			IOutputQueueFactory outputQueueFactory
		) : base(
			categoryName,
			configWrapper,
			outputQueueFactory?.CreateConsoleOutputQueue()
		)
		{
		}
	}
}
