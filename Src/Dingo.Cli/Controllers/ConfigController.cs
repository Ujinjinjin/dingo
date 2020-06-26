using Cliff;
using Dingo.Core.Operations;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Dingo.Cli.Controllers
{
	internal class ConfigController : CliController
	{
		private readonly IConfigOperations _configOperations;
		
		public ConfigController(
			RootCommand rootCommand,
			IConfigOperations configOperations
		) : base(rootCommand)
		{
			_configOperations = configOperations ?? throw new ArgumentNullException(nameof(configOperations));
		}
		
		public override void Register()
		{
			var command = CreateCommand("config", "Group of command that allows to work with configs");
			
			command.AddCommand(CreateCommand(
				"init", 
				"Initialize dingo configuration file",
				CommandHandler.Create<string>(_configOperations.InitConfigurationFileAsync),
				CreateOption(new[] {"--configPath", "-c"}, "Custom path to configuration file", typeof(string), false)
			));
			
			command.AddCommand(CreateCommand(
				"show", 
				"Show current dingo configuration",
				CommandHandler.Create<string>(_configOperations.ShowProjectConfigurationAsync),
				CreateOption(new[] {"--configPath", "-c"}, "Custom path to configuration file", typeof(string), false)
			));
			
			command.AddCommand(CreateCommand("update",
				"Update config file",
				CommandHandler.Create<string, string, string, string, string, string>(_configOperations.UpdateProjectConfigurationAsync),
				CreateOption(new[] {"--configPath", "-c"}, "Custom path to configuration file", typeof(string), false),
				CreateOption(new[] {"--connectionString"}, "Database connection string", typeof(string), false),
				CreateOption(new[] {"--providerName"}, "Database provider name", typeof(string), false),
				CreateOption(new[] {"--migrationSchema"}, "Database schema for you migrations", typeof(string), false),
				CreateOption(new[] {"--migrationTable"}, "Database table, where all migrations are stored", typeof(string), false),
				CreateOption(new[] {"--searchPattern"}, "Pattern to search migration files in specified directoryy", typeof(string), false)
			));
			
			RootCommand.AddCommand(command);
		}
	}
} 