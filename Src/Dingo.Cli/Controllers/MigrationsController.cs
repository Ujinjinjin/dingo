using Cliff;
using Dingo.Core.Services;
using System.CommandLine;
using Cliff.Factories;

namespace Dingo.Cli.Controllers;

/// <summary> Controller allowing to work with migrations </summary>
public sealed class MigrationsController : CliController
{
	private readonly IMigrationService _migrationService;

	public MigrationsController(
		RootCommand rootCommand,
		ICommandFactory commandFactory,
		IOptionFactory optionFactory,
		IMigrationService migrationService
	) : base(
		rootCommand,
		commandFactory,
		optionFactory
	)
	{
		_migrationService = migrationService ?? throw new ArgumentNullException(nameof(migrationService));
	}

	private Command GetNewCommand()
	{
		var nameOption = OptionFactory.CreateOption<string>(
			new[] { "--name", "-n" },
			"Migration name",
			true
		);
		var pathOption = OptionFactory.CreateOption<string>(
			new[] { "--path", "-p" },
			"Path where migration file will be created",
			true
		);

		var command = CommandFactory.CreateCommand(
			"new",
			"Create new migration file",
			nameOption,
			pathOption
		);

		command.SetHandler(
			async (name, path) =>
				await _migrationService.CreateMigrationFileAsync(name, path),
			nameOption,
			pathOption
		);

		return command;
	}

	private Command GetHandshakeCommand()
	{
		var configPathOption = OptionFactory.CreateOption<string>(
			new[] { "--config-path", "-c" },
			"Custom path to configuration file",
			false
		);

		var command = CommandFactory.CreateCommand(
			"handshake",
			"Perform handshake connection to database",
			configPathOption
		);

		command.SetHandler(
			async configPath =>
				await _migrationService.HandshakeDatabaseConnectionAsync(configPath),
			configPathOption
		);

		return command;
	}

	private Command GetRunCommand()
	{
		var migrationsDirOption = OptionFactory.CreateOption<string>(
			new[] { "--migrations-dir", "-m" },
			"Root path to database migration files",
			true
		);
		var configPathOption = OptionFactory.CreateOption<string>(
			new[] { "--config-path", "-c" },
			"Custom path to configuration file",
			false
		);
		var silentOption = OptionFactory.CreateOption<bool>(
			new[] { "--silent", "-s" },
			"Show less info about migration status",
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

		var command = CommandFactory.CreateCommand(
			"run",
			"Apply migrations",
			migrationsDirOption,
			configPathOption,
			silentOption,
			connectionStringOption,
			providerNameOption,
			migrationSchemaOption,
			migrationTableOption
		);

		command.SetHandler(
			async (
				migrationsDir,
				configPath,
				silent,
				connectionString,
				providerName,
				migrationSchema,
				migrationTable
			) => await _migrationService.RunMigrationsAsync(
				migrationsDir,
				configPath,
				silent,
				connectionString,
				providerName,
				migrationSchema,
				migrationTable
			),
			migrationsDirOption,
			configPathOption,
			silentOption,
			connectionStringOption,
			providerNameOption,
			migrationSchemaOption,
			migrationTableOption
		);

		return command;
	}

	private Command GetStatusCommand()
	{
		var migrationsDirOption = OptionFactory.CreateOption<string>(
			new[] { "--migrations-dir", "-m" },
			"Root path to database migration files",
			true
		);
		var configPathOption = OptionFactory.CreateOption<string>(
			new[] { "--config-path", "-c" },
			"Custom path to configuration file",
			false
		);
		var silentOption = OptionFactory.CreateOption<bool>(
			new[] { "--silent", "-s" },
			"Show less info about migration status",
			false
		);

		var command = CommandFactory.CreateCommand(
			"status",
			"Show required actions for migration files",
			migrationsDirOption,
			configPathOption,
			silentOption
		);

		command.SetHandler(
			async (migrationsDir, configPath, silent) =>
				await _migrationService.ShowMigrationsStatusAsync(migrationsDir, configPath, silent),
			migrationsDirOption,
			configPathOption,
			silentOption
		);

		return command;
	}

	/// <inheritdoc />
	public override void Register()
	{
		var command = CommandFactory.CreateCommand("migrations", "Group of commands to work with migrations");

		var subcommandNew = GetNewCommand();
		var subcommandHandshake = GetHandshakeCommand();
		var subcommandRun = GetRunCommand();
		var subcommandStatus = GetStatusCommand();

		command.Add(subcommandNew);
		command.Add(subcommandHandshake);
		command.Add(subcommandRun);
		command.Add(subcommandStatus);

		Register(subcommandNew);
		Register(command);
	}
}