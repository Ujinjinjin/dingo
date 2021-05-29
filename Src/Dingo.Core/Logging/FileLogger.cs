using Dingo.Core.Config;
using Dingo.Core.Extensions;
using Dingo.Core.Factories;
using Dingo.Core.Helpers;
using System;

namespace Dingo.Core.Logging
{
	/// <summary> Logger with output to file </summary>
	internal sealed class FileLogger : LoggerBase
	{
		private readonly IPathHelper _pathHelper;

		protected override string OutputPath => _pathHelper.GetLogsDirectory()
			.ConcatPath($"{DateTime.UtcNow:yyyyMMdd}.log");

		internal FileLogger(
			string categoryName,
			IConfigWrapper configWrapper,
			IOutputQueueFactory outputQueueFactory,
			IPathHelper pathHelper
		) : base(
			categoryName,
			configWrapper,
			outputQueueFactory?.CreateFileOutputQueue()
		)
		{
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
		}
	}
}
