using Cliff;
using Dingo.Core.Operations;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Dingo.Cli.Controllers
{
	/// <summary> Controller allowing to work with project configs </summary>
	internal sealed class ConfigController : CliController
	{
		private readonly IConfigOperations _configOperations;

		public ConfigController(
			RootCommand rootCommand,
			IConfigOperations configOperations
		) : base(rootCommand)
		{
			_configOperations = configOperations ?? throw new ArgumentNullException(nameof(configOperations));
		}

		/// <inheritdoc />
		public override void Register()
		{
			var command = CreateCommand("config", "Group of commands to work with configs");

			command.AddCommand(CreateCommand(
				"init", 
				"Initialize dingo configuration file",
				CommandHandler.Create<string>(_configOperations.InitConfigurationFileAsync),
				CreateOption(new[] {"--config-path", "-c"}, "Custom path to configuration file", typeof(string), false)
			));

			command.AddCommand(CreateCommand(
				"show", 
				"Show current dingo configuration",
				CommandHandler.Create<string>(_configOperations.ShowProjectConfigurationAsync),
				CreateOption(new[] {"--config-path", "-c"}, "Custom path to configuration file", typeof(string), false)
			));

			command.AddCommand(CreateCommand(
				"update",
				"Update config file",
				CommandHandler.Create<string, string, string, string, string, string>(_configOperations.UpdateProjectConfigurationAsync),
				CreateOption(new[] {"--config-path", "-c"}, "Custom path to configuration file", typeof(string), false),
				CreateOption(new[] {"--connection-string"}, "Database connection string", typeof(string), false),
				CreateOption(new[] {"--provider-name"}, "Database provider name", typeof(string), false),
				CreateOption(new[] {"--migration-schema"}, "Database schema for you migrations", typeof(string), false),
				CreateOption(new[] {"--migration-table"}, "Database table, where all migrations are stored", typeof(string), false),
				CreateOption(new[] {"--search-pattern"}, "Pattern to search migration files in specified directory", typeof(string), false)
			));

			RootCommand.AddCommand(command);
		}
	}
} 