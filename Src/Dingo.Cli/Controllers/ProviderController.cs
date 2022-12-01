using Cliff;
using Dingo.Core.Services;
using System.CommandLine;
using Cliff.Factories;

namespace Dingo.Cli.Controllers;

/// <summary> Controller allowing to manage database provider </summary>
internal sealed class ProviderController : CliController
{
	private readonly IProviderService _providerService;

	public ProviderController(
		RootCommand rootCommand,
		ICommandFactory commandFactory,
		IOptionFactory optionFactory,
		IProviderService providerService
	) : base(
		rootCommand,
		commandFactory,
		optionFactory
	)
	{
		_providerService = providerService ?? throw new ArgumentNullException(nameof(providerService));
	}

	private Command GetChooseCommand()
	{
		var configPathOption = OptionFactory.CreateOption<string>(
			new[] { "--config-path", "-c" },
			"Custom path to configuration file",
			false
		);

		var command = CommandFactory.CreateCommand(
			"choose",
			"Choose database provider from supported list",
			configPathOption
		);

		command.SetHandler(
			async configPath =>
				await _providerService.ChooseDatabaseProviderAsync(configPath),
			configPathOption
		);

		return command;
	}

	private Command GetListCommand()
	{
		var command = CommandFactory.CreateCommand(
			"list",
			"Display list of supported database providers"
		);
		
		command.SetHandler(
			async () => await _providerService.ListSupportedDatabaseProvidersAsync()
		);

		return command;
	}

	private Command GetValidateCommand()
	{
		var configPathOption = OptionFactory.CreateOption<string>(
			new[] { "--config-path", "-c" },
			"Custom path to configuration file",
			false
		);

		var command = CommandFactory.CreateCommand(
			"validate",
			"Validate chosen database provider",
			configPathOption
		);
		
		command.SetHandler(
			async configPath =>
				await _providerService.ValidateDatabaseProviderAsync(configPath),
			configPathOption
		);

		return command;
	}

	/// <inheritdoc />
	public override void Register()
	{
		var command = CommandFactory.CreateCommand("provider", "Group of commands to manage database provider");

		var subcommandChoose = GetChooseCommand();
		var subcommandList = GetListCommand();
		var subcommandValidate = GetValidateCommand();

		command.Add(subcommandChoose);
		command.Add(subcommandList);
		command.Add(subcommandValidate);
		
		Register(command);
	}
}