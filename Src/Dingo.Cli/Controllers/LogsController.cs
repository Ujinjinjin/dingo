using Cliff;
using Dingo.Core.Services;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Dingo.Cli.Controllers;

/// <summary> Controller allowing to work with logs </summary>
internal sealed class LogsController : CliController
{
	private readonly ILogsService _logsService;

	public LogsController(RootCommand rootCommand, ILogsService logsService) : base(rootCommand)
	{
		_logsService = logsService ?? throw new ArgumentNullException(nameof(logsService));
	}

	public override void Register()
	{
		var command = CreateCommand("logs", "Group of commands to work with logs");

		command.AddCommand(CreateCommand(
			"level", 
			"Switch level of logging",
			CommandHandler.Create<string, int?>(_logsService.SwitchLogLevelAsync),
			CreateOption(new[] {"--config-path", "-c"}, "Custom path to configuration file", typeof(string), false),
			CreateOption(new[] {"--log-level", "-l"}, "Integer representation of logging level", typeof(int?), false)
		));

		command.AddCommand(CreateCommand(
			"prune", 
			"Prune dingo log files",
			CommandHandler.Create(_logsService.PruneLogsAsync)
		));

		RootCommand.AddCommand(command);
	}
}