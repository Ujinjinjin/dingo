using System.CommandLine;
using Cliff;
using Cliff.Factories;
using Dingo.Core.Extensions;
using Dingo.Core.Services.Handlers;

namespace Dingo.Cli.Controllers;

internal sealed class LogsController : CliController
{
	private readonly ILogsHandler _logsHandler;

	public LogsController(
		RootCommand rootCommand,
		ICommandFactory commandFactory,
		IOptionFactory optionFactory,
		ILogsHandler logsHandler
	) : base(
		rootCommand,
		commandFactory,
		optionFactory
	)
	{
		_logsHandler = logsHandler.Required(nameof(logsHandler));
	}

	public override void Register()
	{
		var command = CommandFactory.CreateCommand("logs", "Group of commands to work with logs");
		command.Add(GetPruneCommand());
		Register(command);
	}

	private Command GetPruneCommand()
	{
		var command = CommandFactory.CreateCommand(
			"prune",
			"Prune dingo log files"
		);

		command.SetHandler(() => _logsHandler.Prune());

		return command;
	}
}
