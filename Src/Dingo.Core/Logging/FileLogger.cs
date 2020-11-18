using Dingo.Core.Factories;
using Dingo.Core.Helpers;
using Dingo.Core.Helpers.Static;
using Dingo.Core.IO;
using Microsoft.Extensions.Logging;
using System;

namespace Dingo.Core.Logging
{
	/// <summary> Logger with output to file </summary>
	internal class FileLogger : ILogger
	{
		private readonly string _categoryName;
		private readonly LogLevel _logLevel;
		private readonly IOutputQueue _outputQueue;
		private readonly IPathHelper _pathHelper;
		
		internal FileLogger(string categoryName, LogLevel logLevel, IOutputQueueFactory outputQueueFactory, IPathHelper pathHelper)
		{
			_categoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
			_logLevel = logLevel;
			_outputQueue = outputQueueFactory.CreateFileOutputQueue() ?? throw new ArgumentNullException(nameof(outputQueueFactory));
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
		}

		/// <inheritdoc />
		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
			{
				return;
			}

			if (formatter == null)
			{
				throw new ArgumentNullException(nameof(formatter));
			}
			
			var message = formatter(state, exception);
			if (exception != null)
			{
				message += $"{Environment.NewLine}{Environment.NewLine}{exception}";
			}

			if (string.IsNullOrEmpty(message))
			{
				return;
			}

			message = $"{logLevel} | {DateTime.UtcNow} | {_categoryName} | {message}";
			var outputPath = $"{_pathHelper.GetApplicationBaseDirectory()}/logs/{DateTime.Now:yyyyMMdd}.log";
			
			_outputQueue.EnqueueOutput(outputPath, message);
		}
		
		/// <inheritdoc />
		public bool IsEnabled(LogLevel logLevel)
		{
			return logLevel >= _logLevel;
		}

		/// <inheritdoc />
		public IDisposable BeginScope<TState>(TState state)
		{
			return DisposableHelper.Empty;
		}
	}
}
