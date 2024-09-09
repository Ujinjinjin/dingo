using Dingo.Core.Extensions;
using Dingo.Core.IO;
using Dingo.Core.Services.Logs;
using Dingo.Core.Utils;
using Microsoft.Extensions.Logging;

namespace Dingo.Core.Services.Handlers;

public sealed class LogsHandler : ILogsHandler
{
	private readonly ILogsPruner _logsPruner;
	private readonly IOutput _output;
	private readonly ILogger _logger;

	public LogsHandler(
		ILogsPruner logsPruner,
		IOutput output,
		ILoggerFactory loggerFactory
	)
	{
		_logsPruner = logsPruner.Required(nameof(logsPruner));
		_output = output.Required(nameof(output));
		_logger = loggerFactory.Required(nameof(loggerFactory))
			.CreateLogger<LogsHandler>()
			.Required(nameof(loggerFactory));
	}

	public void Prune()
	{
		using var _ = new CodeTiming(_logger);

		try
		{
			_logsPruner.Prune();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "LogsHandler:Prune:Error");
			_output.Write("Error occured while pruning logs", LogLevel.Error);
		}
	}
}
