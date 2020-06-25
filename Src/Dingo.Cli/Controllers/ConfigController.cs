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
			
			command.AddCommand(CreateInitAsyncCommand());
			command.AddCommand(CreateShowAsyncCommand());
			command.AddCommand(CreateUpdateAsyncCommand());
			
			RootCommand.AddCommand(command);
		}

		private Command CreateInitAsyncCommand()
		{
			var command = CreateCommand(
				"init", 
				"Initialize dingo configuration file",
				CreateOption(new[] {"--configPath", "-c"}, "Custom path to configuration file", typeof(string), false)
			);
			command.Handler = CommandHandler.Create<string>(_configOperations.InitAsync);
			return command;
		}

		private Command CreateShowAsyncCommand()
		{
			var command = CreateCommand(
				"show", 
				"Show current dingo configuration",
				CreateOption(new[] {"--configPath", "-c"}, "Custom path to configuration file", typeof(string), false)
			);
			command.Handler = CommandHandler.Create<string>(_configOperations.ShowAsync);
			return command;
		}

		private Command CreateUpdateAsyncCommand()
		{
			var command = CreateCommand("update",
				"Update config file",
				CreateOption(new[] {"--configPath", "-c"}, "Custom path to configuration file", typeof(string), false),
				CreateOption(new[] {"--connectionString"}, "Database connection string", typeof(string), false),
				CreateOption(new[] {"--providerName"}, "Database provider name", typeof(string), false),
				CreateOption(new[] {"--migrationSchema"}, "Database schema for you migrations", typeof(string), false),
				CreateOption(new[] {"--migrationTable"}, "Database table, where all migrations are stored", typeof(string), false),
				CreateOption(new[] {"--searchPattern"}, "Pattern to search migration files in specified directoryy", typeof(string), false)
			);

			command.Handler = CommandHandler.Create<string, string, string, string, string, string>(_configOperations.UpdateAsync);

			return command;
		}
	}
} 