using System.CommandLine;
using Cliff;
using Cliff.Factories;
using Dingo.Core.Extensions;
using Dingo.Core.Services.Handlers;

namespace Dingo.Cli.Controllers;

internal sealed class ConnectionController : CliController
{
	private readonly IConnectionHandler _connectionHandler;

	public ConnectionController(
		RootCommand rootCommand,
		ICommandFactory commandFactory,
		IOptionFactory optionFactory,
		IConnectionHandler connectionHandler
	) : base(
		rootCommand,
		commandFactory,
		optionFactory
	)
	{
		_connectionHandler = connectionHandler.Required(nameof(connectionHandler));
	}

	public override void Register()
	{
		var command = CommandFactory.CreateCommand("db", "Group of commands to work with db");
		command.Add(GetPingCommand());
		Register(command);
	}

	private Command GetPingCommand()
	{
		var command = CommandFactory.CreateCommand(
			"ping",
			"Ping database to check its availability"
		);

		command.SetHandler(async () => await _connectionHandler.HandshakeAsync());

		return command;
	}
}
