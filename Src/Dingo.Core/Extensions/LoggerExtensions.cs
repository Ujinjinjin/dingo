using Microsoft.Extensions.Logging;
using System;

namespace Dingo.Core.Extensions;

/// <summary> Collection of extensions for <see cref="ILogger"/> </summary>
internal static class LoggerExtensions
{
	/// <summary> Log message with custom log level </summary>
	/// <param name="logger">Logger</param>
	/// <param name="logLevel">Log level</param>
	/// <param name="message">Message to be logged</param>
	/// <param name="exception">Exception to be logged</param>
	public static void LogCustom(this ILogger logger, LogLevel logLevel, string message, Exception exception = null)
	{
		logger.Log(logLevel, exception, message);
	}

	/// <summary> Log message with <see cref="LogLevel.Debug"/> log level </summary>
	/// <param name="logger">Logger</param>
	/// <param name="message">Message to be logged</param>
	public static void LogDebug(this ILogger logger, string message)
	{
		logger.Log(LogLevel.Debug, message);
	}

	/// <summary> Log message with <see cref="LogLevel.Error"/> log level </summary>
	/// <param name="logger">Logger</param>
	/// <param name="exception">Exception to be logged</param>
	/// <param name="message">Message to be logged</param>
	public static void LogError(this ILogger logger, Exception exception, string message)
	{
		logger.Log(LogLevel.Error, exception, message);
	}
}