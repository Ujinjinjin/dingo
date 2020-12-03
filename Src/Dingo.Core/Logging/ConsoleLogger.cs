using Dingo.Core.Factories;
using Microsoft.Extensions.Logging;

namespace Dingo.Core.Logging
{
	/// <summary> Logger with output to file </summary>
	internal class ConsoleLogger : LoggerBase
	{
		public ConsoleLogger(
			string categoryName,
			LogLevel logLevel,
			IOutputQueueFactory outputQueueFactory
		) : base(
			categoryName,
			logLevel,
			outputQueueFactory?.CreateConsoleOutputQueue()
		)
		{
		}
	}
}
