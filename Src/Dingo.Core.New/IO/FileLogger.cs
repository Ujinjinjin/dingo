using System.Text;
using Dingo.Core.Extensions;
using Dingo.Core.Services.Adapters;
using Microsoft.Extensions.Logging;
using Trico.Configuration;

namespace Dingo.Core.IO;

internal class FileLogger : ILogger
{
	private readonly string _categoryName;
	private readonly IConfiguration _configuration;
	private readonly IFileAdapter _fileAdapter;
	private readonly IDirectoryAdapter _directoryAdapter;
	private readonly IPathAdapter _pathAdapter;

	private readonly StringBuilder _logMessageBuilder;

	public FileLogger(
		string categoryName,
		IConfiguration configuration,
		IFileAdapter fileAdapter,
		IDirectoryAdapter directoryAdapter,
		IPathAdapter pathAdapter
	)
	{
		_categoryName = categoryName.Required(nameof(categoryName));
		_configuration = configuration.Required(nameof(configuration));
		_fileAdapter = fileAdapter.Required(nameof(fileAdapter));
		_directoryAdapter = directoryAdapter.Required(nameof(directoryAdapter));
		_pathAdapter = pathAdapter.Required(nameof(pathAdapter));

		_logMessageBuilder = new StringBuilder();
	}

	public void Log<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter
	)
	{
		if (!IsEnabled(logLevel))
		{
			return;
		}

		var logFilePath = GetLogFilePath();
		var logMessage = BuildLogMessage(logLevel, eventId, state, exception, formatter);

		using var writer = _fileAdapter.AppendText(logFilePath);
		writer.WriteLine(logMessage);
	}

	private string GetLogFilePath()
	{
		var logsDir = _pathAdapter.GetLogsPath();
		if (!_directoryAdapter.Exists(logsDir))
		{
			_ = _directoryAdapter.CreateDirectory(logsDir);
		}

		var logDate = DateTime.UtcNow.ToString("yyyyMMdd");
		var logFilePath = _pathAdapter.Join(logsDir, $"{logDate}.log");
		if (!_fileAdapter.Exists(logFilePath))
		{
			_fileAdapter.Create(logFilePath).Close();
		}

		return logFilePath;
	}

	private string BuildLogMessage<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter
	)
	{
		_logMessageBuilder.Clear();
		if (formatter == null)
		{
			throw new ArgumentNullException(nameof(formatter));
		}

		_logMessageBuilder.Append(formatter(state, exception));
		if (exception != null)
		{
			_logMessageBuilder.Append("; Exception:");
			_logMessageBuilder.Append(Environment.NewLine);
			_logMessageBuilder.Append(exception);
		}

		if (string.IsNullOrEmpty(_logMessageBuilder.ToString()))
		{
			return string.Empty;
		}

		var dateTimeString = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

		return $"{dateTimeString} | {logLevel.ToString()} | {_categoryName} | {_logMessageBuilder}";
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		if (!Enum.TryParse<LogLevel>(_configuration.Get(Configuration.Key.LogLevel), out var allowedLogLevel))
		{
			return false;
		}

		return logLevel == LogLevel.None || logLevel >= allowedLogLevel;
	}

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull
	{
		return null;
	}
}
