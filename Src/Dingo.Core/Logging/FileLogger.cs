using Dingo.Core.Factories;
using Dingo.Core.Helpers;
using Microsoft.Extensions.Logging;
using System;

namespace Dingo.Core.Logging
{
	/// <summary> Logger with output to file </summary>
	internal class FileLogger : LoggerBase
	{
		private readonly IPathHelper _pathHelper;

		protected override string OutputPath => $"{_pathHelper.GetApplicationBaseDirectory()}/logs/{DateTime.UtcNow:yyyyMMdd}.log";

		internal FileLogger(
			string categoryName,
			LogLevel logLevel,
			IOutputQueueFactory outputQueueFactory,
			IPathHelper pathHelper
		) : base(
			categoryName,
			logLevel,
			outputQueueFactory?.CreateFileOutputQueue()
		)
		{
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
		}
	}
}
