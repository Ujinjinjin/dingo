using Dingo.Core.Config;
using Dingo.Core.Helpers.Static;
using Dingo.Core.IO;
using Microsoft.Extensions.Logging;
using System;

namespace Dingo.Core.Logging
{
	/// <summary> Base class for dingo loggers </summary>
	internal abstract class LoggerBase : ILogger
	{
		private readonly string _categoryName;
		private readonly IConfigWrapper _configWrapper;
		private readonly IOutputQueue _outputQueue;

		protected virtual string OutputPath => null;

		protected LoggerBase(
			string categoryName,
			IConfigWrapper configWrapper,
			IOutputQueue outputQueue
		)
		{
			_categoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
			_outputQueue = outputQueue ?? throw new ArgumentNullException(nameof(outputQueue));
			_configWrapper = configWrapper;
		}

		/// <inheritdoc />
		public void Log<TState>(
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception exception,
			Func<TState, Exception, string> formatter
		)
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
				message += Environment.NewLine + Environment.NewLine + exception;
			}

			if (string.IsNullOrEmpty(message))
			{
				return;
			}

			message = $"{logLevel} | {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | {_categoryName} | {message}";

			_outputQueue.EnqueueOutput(message, OutputPath);
		}

		/// <inheritdoc />
		public bool IsEnabled(LogLevel logLevel)
		{
			return logLevel >= (LogLevel) (_configWrapper.LogLevel ?? (int) LogLevel.None);
		}

		/// <inheritdoc />
		public IDisposable BeginScope<TState>(TState state)
		{
			return DisposableHelper.Empty;
		}
	}
}
