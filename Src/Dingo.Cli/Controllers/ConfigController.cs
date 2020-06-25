using Cliff;
using Dingo.Core.Config;
using Dingo.Core.Renderer;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Dingo.Cli.Controllers
{
	internal class ConfigController : CliController
	{
		private readonly IConfigWrapper _configWrapper;
		private readonly IRenderer _renderer;
		
		public ConfigController(
			RootCommand rootCommand,
			IConfigWrapper configWrapper,
			IRenderer renderer
		) : base(rootCommand)
		{
			_configWrapper = configWrapper ?? throw new ArgumentNullException(nameof(configWrapper));
			_renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
		}
		
		public override void Register()
		{
			var command = Command("config", "Group of command that allows to work with configs");
			
			command.AddCommand(GetInitAsyncCommand());
			command.AddCommand(GetShowAsyncCommand());
			command.AddCommand(GetUpdateAsyncCommand());
			
			RootCommand.AddCommand(command);
		}
		
		private async Task InitAsync(string configPath = null)
		{
			await _configWrapper.LoadAsync(configPath);
			_configWrapper.ConnectionString = string.Empty;
			_configWrapper.ProviderName = string.Empty;
			await _configWrapper.SaveAsync(configPath);
			await _renderer.ShowMessage("Dingo config file successfuly initialized!");
		}

		private Command GetInitAsyncCommand()
		{
			var command = Command(
				"init", 
				"Initialize dingo configuration file",
				Option(new[] {"--configPath", "-c"}, "Custom path to configuration file", typeof(string), false)
			);
			command.Handler = CommandHandler.Create<string>(InitAsync);
			return command;
		}

		private async Task ShowAsync(string configPath = null)
		{
			await _configWrapper.LoadAsync(configPath);
			await _renderer.ShowConfigAsync(_configWrapper);
		}

		private Command GetShowAsyncCommand()
		{
			var command = Command(
				"show", 
				"Show current dingo configuration",
				Option(new[] {"--configPath", "-c"}, "Custom path to configuration file", typeof(string), false)
			);
			command.Handler = CommandHandler.Create<string>(ShowAsync);
			return command;
		}

		private async Task UpdateAsync(
			string configPath = null,
			string connectionString = null,
			string providerName = null,
			string migrationSchema = null,
			string migrationTable = null,
			string searchPattern = null
		)
		{
			await _configWrapper.LoadAsync(configPath);

			_configWrapper.ConnectionString = string.IsNullOrWhiteSpace(connectionString)
				? _configWrapper.ConnectionString
				: connectionString;
			
			_configWrapper.ProviderName = string.IsNullOrWhiteSpace(providerName)
				? _configWrapper.ProviderName
				: providerName;
			
			_configWrapper.MigrationSchema = string.IsNullOrWhiteSpace(migrationSchema)
				? _configWrapper.MigrationSchema
				: migrationSchema;
			
			_configWrapper.MigrationTable = string.IsNullOrWhiteSpace(migrationTable)
				? _configWrapper.MigrationTable
				: migrationTable;
			
			_configWrapper.MigrationsSearchPattern = string.IsNullOrWhiteSpace(searchPattern)
				? _configWrapper.MigrationsSearchPattern
				: searchPattern;

			await _configWrapper.SaveAsync(configPath);
		}

		private Command GetUpdateAsyncCommand()
		{
			var command = Command("update",
				"Update config file",
				Option(new[] {"--configPath", "-c"}, "Custom path to configuration file", typeof(string), false),
				Option(new[] {"--connectionString"}, "Connection string", typeof(string), false),
				Option(new[] {"--providerName"}, "Provider name. To list supported provider names use `dingo provider list`", typeof(string), false),
				Option(new[] {"--migrationSchema"}, "DB schema containing migrations table (defalt `public`)", typeof(string), false),
				Option(new[] {"--migrationTable"}, "DB table where migrations registred (default `dingo_migration`)", typeof(string), false),
				Option(new[] {"--searchPattern"}, "Search pattern used to find migrations within specified directory", typeof(string), false)
			);

			command.Handler = CommandHandler.Create<string, string, string, string, string, string>(UpdateAsync);

			return command;
		}
	}
} 