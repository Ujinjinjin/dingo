using System.CommandLine;
using Cliff;
using Cliff.Factories;
using Dingo.Core.Extensions;
using Dingo.Core.Services.Handlers;

namespace Dingo.Cli.Controllers;

public class MigrationController : CliController
{
	private readonly IMigrationHandler _migrationHandler;

	public MigrationController(
		RootCommand rootCommand,
		ICommandFactory commandFactory,
		IOptionFactory optionFactory,
		IMigrationHandler migrationHandler
	) : base(
		rootCommand,
		commandFactory,
		optionFactory
	)
	{
		_migrationHandler = migrationHandler.Required(nameof(migrationHandler));
	}

	/// <inheritdoc />
	public override void Register()
	{
		Register(GetCreateCommand());
		Register(GetUpCommand());
		Register(GetDownCommand());
		Register(GetStatusCommand());
	}

	private Command GetCreateCommand()
	{
		var nameOption = OptionFactory.CreateOption<string>(
			new[] { "--name", "-n" },
			"Migration name",
			true
		);
		var pathOption = OptionFactory.CreateOption<string>(
			new[] { "--path", "-p" },
			"Destination where migration file will be created",
			true
		);

		var command = CommandFactory.CreateCommand(
			"new",
			"Create new migration file",
			nameOption,
			pathOption
		);

		command.SetHandler(
			async (name, path) => await _migrationHandler.CreateAsync(name, path),
			nameOption,
			pathOption
		);

		return command;
	}

	private Command GetUpCommand()
	{
		var profileOption = OptionFactory.CreateOption<string?>(
			new[] { "--configuration", "-c" },
			"Configuration profile name",
			false
		);
		var pathOption = OptionFactory.CreateOption<string?>(
			new[] { "--path", "-p" },
			"Root path to database migration files",
			false
		);

		var command = CommandFactory.CreateCommand(
			"up",
			"Apply all outdated migrations up",
			profileOption,
			pathOption
		);

		command.SetHandler(
			async (profile, path) => await _migrationHandler.MigrateAsync(profile, path),
			profileOption,
			pathOption
		);

		return command;
	}

	private Command GetDownCommand()
	{
		var profileOption = OptionFactory.CreateOption<string?>(
			new[] { "--configuration", "-c" },
			"Configuration profile name",
			false
		);
		var pathOption = OptionFactory.CreateOption<string?>(
			new[] { "--path", "-p" },
			"Root path to database migration files",
			false
		);
		var countOption = OptionFactory.CreateOption<int?>(
			new[] { "--count" },
			"Number of patches to rollback. Default: 1",
			false
		);
		var forceOption = OptionFactory.CreateOption<bool>(
			new[] { "--force", "-f" },
			"Ignore all warnings and rollback patches",
			false
		);

		var command = CommandFactory.CreateCommand(
			"down",
			"Rollback N last patches",
			profileOption,
			pathOption,
			countOption,
			forceOption
		);

		command.SetHandler(
			async (profile, path, count, force) => await _migrationHandler.RollbackAsync(profile, path, count, force),
			profileOption,
			pathOption,
			countOption,
			forceOption
		);

		return command;
	}

	private Command GetStatusCommand()
	{
		var profileOption = OptionFactory.CreateOption<string?>(
			new[] { "--configuration", "-c" },
			"Configuration profile name",
			false
		);
		var pathOption = OptionFactory.CreateOption<string?>(
			new[] { "--path", "-p" },
			"Root path to database migration files",
			false
		);

		var command = CommandFactory.CreateCommand(
			"status",
			"Show status of migrations in working directory",
			profileOption,
			pathOption
		);

		command.SetHandler(
			async (profile, path) => await _migrationHandler.ShowStatusAsync(profile, path),
			profileOption,
			pathOption
		);

		return command;
	}
}
