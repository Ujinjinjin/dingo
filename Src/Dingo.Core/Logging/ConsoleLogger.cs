using Dingo.Core.Helpers.Static;
using Microsoft.Extensions.Logging;
using System;

namespace Dingo.Core.Logging
{
	internal class ConsoleLogger : ILogger
	{
		private readonly string _categoryName;
		private readonly LogLevel _logLevel;

		public ConsoleLogger(string categoryName, LogLevel logLevel)
		{
			_categoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
			_logLevel = logLevel;
		}

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
				message += Environment.NewLine + Environment.NewLine + exception;
			}

			if (string.IsNullOrEmpty(message))
			{
				return;
			}

			message = $"{logLevel} | {DateTime.UtcNow} | {_categoryName} | {message}";
			
			Console.WriteLine(message);
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return logLevel >= _logLevel;
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return DisposableHelper.Empty;
		}
	}
}
