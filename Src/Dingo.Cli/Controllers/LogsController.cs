using Cliff;
using Dingo.Core.Services;
using System.CommandLine;
using Cliff.Factories;

namespace Dingo.Cli.Controllers;

/// <summary> Controller allowing to work with logs </summary>
internal sealed class LogsController : CliController
{
	private readonly ILogsService _logsService;

	public LogsController(
		RootCommand rootCommand,
		ICommandFactory commandFactory,
		IOptionFactory optionFactory,
		ILogsService logsService
	) : base(
		rootCommand,
		commandFactory,
		optionFactory
	)
	{
		_logsService = logsService ?? throw new ArgumentNullException(nameof(logsService));
	}

	private Command GetLevelCommand()
	{
		var configPathOption = OptionFactory.CreateOption<string>(
			new[] { "--config-path", "-c" },
			"Custom path to configuration file",
			false
		);
		var logLevelOption = OptionFactory.CreateOption<int?>(
			new[] { "--log-level", "-l" },
			"Integer representation of logging level",
			false
		);

		var command = CommandFactory.CreateCommand(
			"level", 
			"Switch level of logging",
			configPathOption,
			logLevelOption
		);
		
		command.SetHandler(
			async (configPath, logLevel) =>
				await _logsService.SwitchLogLevelAsync(configPath, logLevel),
			configPathOption,
			logLevelOption
		);

		return command;
	}

	private Command GetPruneCommand()
	{
		var command = CommandFactory.CreateCommand(
			"prune", 
			"Prune dingo log files"
		);
		
		command.SetHandler(async () => await _logsService.PruneLogsAsync());

		return command;
	}

	public override void Register()
	{
		var command = CommandFactory.CreateCommand("logs", "Group of commands to work with logs");

		var subcommandLevel = GetLevelCommand();
		var subcommandPrune = GetPruneCommand();
		
		command.Add(subcommandLevel);
		command.Add(subcommandPrune);

		Register(command);
	}
}