using Cliff;
using Dingo.Core.Services;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Dingo.Cli.Controllers;

/// <summary> Controller allowing to manage database provider </summary>
internal sealed class ProviderController : CliController
{
	private readonly IProviderService _providerService;

	public ProviderController(
		RootCommand rootCommand,
		IProviderService providerService
	) : base(rootCommand)
	{
		_providerService = providerService ?? throw new ArgumentNullException(nameof(providerService));
	}

	/// <inheritdoc />
	public override void Register()
	{
		var command = CreateCommand("provider", "Group of commands to manage database provider");

		command.AddCommand(CreateCommand(
			"choose",
			"Choose database provider from supported list",
			CommandHandler.Create<string>(_providerService.ChooseDatabaseProviderAsync),
			CreateOption(new[] {"--config-path", "-c"}, "Custom path to configuration file", typeof(string), false)
		));

		command.AddCommand(CreateCommand(
			"list",
			"Display list of supported database providers",
			CommandHandler.Create(_providerService.ListSupportedDatabaseProvidersAsync)
		));

		command.AddCommand(CreateCommand(
			"validate",
			"Validate chosen database provider",
			CommandHandler.Create<string>(_providerService.ValidateDatabaseProviderAsync),
			CreateOption(new[] {"--config-path", "-c"}, "Custom path to configuration file", typeof(string), false)
		));

		RootCommand.AddCommand(command);
	}
}