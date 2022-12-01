using Cliff;
using Dingo.Core.Services;
using System.CommandLine;
using Cliff.Factories;

namespace Dingo.Cli.Controllers;

/// <summary> Controller allowing to work with project configs </summary>
internal sealed class ConfigController : CliController
{
	private readonly IConfigService _configService;

	public ConfigController(
		RootCommand rootCommand,
		ICommandFactory commandFactory,
		IOptionFactory optionFactory,
		IConfigService configService
	) : base(
		rootCommand,
		commandFactory,
		optionFactory
	)
	{
		_configService = configService ?? throw new ArgumentNullException(nameof(configService));
	}
	
	private Command GetInitCommand()
	{
		var configPathOption = OptionFactory.CreateOption<string>(
			new[] { "--config-path", "-c" },
			"Custom path to configuration file",
			false
		);

		var command = CommandFactory.CreateCommand(
			"init", 
			"Initialize dingo configuration file",
			configPathOption
		);

		command.SetHandler(
			async configPath =>
				await _configService.InitConfigurationFileAsync(configPath),
			configPathOption
		);

		return command;
	}
	
	private Command GetShowCommand()
	{
		var configPathOption = OptionFactory.CreateOption<string>(
			new[] { "--config-path", "-c" },
			"Custom path to configuration file",
			false
		);

		var command = CommandFactory.CreateCommand(
			"new",
			"Create new migration file",
			configPathOption
		);

		command.SetHandler(
			async configPath =>
				await _configService.ShowProjectConfigurationAsync(configPath),
			configPathOption
		);

		return command;
	}
	
	private Command GetUpdateCommand()
	{
		var configPathOption = OptionFactory.CreateOption<string>(
			new[] { "--config-path", "-c" },
			"Custom path to configuration file",
			false
		);
		var connectionStringOption = OptionFactory.CreateOption<string>(
			new[] { "--connection-string" },
			"Database connection string",
			false
		);
		var providerNameOption = OptionFactory.CreateOption<string>(
			new[] { "--provider-name" },
			"Database provider name",
			false
		);
		var migrationSchemaOption = OptionFactory.CreateOption<string>(
			new[] { "--migration-schema" },
			"Database schema for you migrations",
			false
		);
		var migrationTableOption = OptionFactory.CreateOption<string>(
			new[] { "--migration-table" },
			"Database table, where all migrations are stored",
			false
		);
		var searchPatternOption = OptionFactory.CreateOption<string>(
			new[] { "--search-pattern" },
			"Pattern to search migration files in specified directory",
			false
		);

		var command = CommandFactory.CreateCommand(
			"update",
			"Update config file",
			configPathOption,
			connectionStringOption,
			providerNameOption,
			migrationSchemaOption,
			migrationTableOption,
			searchPatternOption
		);

		command.SetHandler(
			async (
				configPath,
				connectionString,
				providerName,
				migrationSchema,
				migrationTable,
				searchPattern
			) => await _configService.UpdateProjectConfigurationAsync(
				configPath,
				connectionString,
				providerName,
				migrationSchema,
				migrationTable,
				searchPattern
			),
			configPathOption,
			connectionStringOption,
			providerNameOption,
			migrationSchemaOption,
			migrationTableOption,
			searchPatternOption
		);

		return command;
	}

	/// <inheritdoc />
	public override void Register()
	{
		var command = CommandFactory.CreateCommand("config", "Group of commands to work with configs");

		var subcommandInit = GetInitCommand();
		var subcommandShow = GetShowCommand();
		var subcommandUpdate = GetUpdateCommand();

		command.Add(subcommandInit);
		command.Add(subcommandShow);
		command.Add(subcommandUpdate);

		Register(command);
	}
}