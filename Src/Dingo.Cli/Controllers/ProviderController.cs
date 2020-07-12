using Cliff;
using Dingo.Core.Operations;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Dingo.Cli.Controllers
{
	internal class ProviderController : CliController
	{
		private readonly IProviderOperations _providerOperations;

		public ProviderController(
			RootCommand rootCommand,
			IProviderOperations providerOperations
		) : base(rootCommand)
		{
			_providerOperations = providerOperations ?? throw new ArgumentNullException(nameof(providerOperations));
		}

		public override void Register()
		{
			var command = CreateCommand("provider", "Group of commands to manage database provider");

			command.AddCommand(CreateCommand(
				"list",
				"Display list of supported database providers",
				CommandHandler.Create(_providerOperations.ListSupportedDatabaseProvidersAsync)
			));

			command.AddCommand(CreateCommand(
				"choose",
				"Choose database provider from supported list",
				CommandHandler.Create<string>(_providerOperations.ChooseDatabaseProviderAsync),
				CreateOption(new[] {"--configPath", "-c"}, "Custom path to configuration file", typeof(string), false)
			));

			command.AddCommand(CreateCommand(
				"validate",
				"Validate chosen database provider",
				CommandHandler.Create<string>(_providerOperations.ValidateDatabaseProviderAsync),
				CreateOption(new[] {"--configPath", "-c"}, "Custom path to configuration file", typeof(string), false)
			));

			RootCommand.AddCommand(command);
		}
	}
}
