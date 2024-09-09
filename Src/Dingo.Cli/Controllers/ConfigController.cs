using System.CommandLine;
using Cliff;
using Cliff.Factories;
using Dingo.Core.Extensions;
using Dingo.Core.Services.Handlers;

namespace Dingo.Cli.Controllers;

internal sealed class ConfigController : CliController
{
	private readonly IConfigHandler _configHandler;

	public ConfigController(
		RootCommand rootCommand,
		ICommandFactory commandFactory,
		IOptionFactory optionFactory,
		IConfigHandler configHandler
	) : base(
		rootCommand,
		commandFactory,
		optionFactory
	)
	{
		_configHandler = configHandler.Required(nameof(configHandler));
	}

	/// <inheritdoc />
	public override void Register()
	{
		Register(GetInitCommand());
	}

	private Command GetInitCommand()
	{
		var pathOption = OptionFactory.CreateOption<string>(
			new[] { "--path", "-p" },
			"Destination where configuration directory and files will be created. Default: current directory",
			false
		);
		var profileOption = OptionFactory.CreateOption<string>(
			new[] { "--configuration", "-c" },
			"Configuration profile name",
			false
		);

		var command = CommandFactory.CreateCommand(
			"init",
			"Initialize dingo configuration profile",
			pathOption,
			profileOption
		);

		command.SetHandler(
			(path, profile) => _configHandler.Init(path, profile),
			pathOption,
			profileOption
		);

		return command;
	}
}
