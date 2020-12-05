using Dingo.Core.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Dingo.Core.DbUtils
{
	/// <summary> Disposable query execution scope </summary>
	internal class QueryExecutionScope : IDisposable
	{
		private readonly Stopwatch _stopwatch;
		private readonly ILogger _logger;
		private readonly string _scopeGuid;

		public QueryExecutionScope(ILogger logger)
		{
			_stopwatch = Stopwatch.StartNew();
			_scopeGuid = Guid.NewGuid().ToString("N");
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <inheritdoc />
		public void Dispose()
		{
			_stopwatch.Stop();
		}

		/// <summary> Log message </summary>
		/// <param name="logLevel">Log level</param>
		/// <param name="message">Message to log</param>
		/// <param name="exception">Exception</param>
		public void Log(LogLevel logLevel, string message, Exception exception = null)
		{
			_logger.LogCustom(logLevel, $"{_scopeGuid}: {message}", exception);
		}
	}
}
