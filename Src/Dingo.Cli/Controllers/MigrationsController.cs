using Cliff;
using Dingo.Core.Operations;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Dingo.Cli.Controllers
{
	/// <summary> Controller allowing to work with migrations </summary>
	public class MigrationsController : CliController
	{
		private readonly IMigrationOperations _migrationOperations;

		public MigrationsController(
			RootCommand rootCommand,
			IMigrationOperations migrationOperations
		) : base(rootCommand)
		{
			_migrationOperations = migrationOperations ?? throw new ArgumentNullException(nameof(migrationOperations));
		}

		/// <inheritdoc />
		public override void Register()
		{
			var command = CreateCommand("migrations", "Group of commands to work with migrations");

			var subCommandNew = CreateCommand(
				"new",
				"Create new migration file",
				CommandHandler.Create<string, string>(_migrationOperations.CreateMigrationFileAsync),
				CreateOption(new[] {"--name", "-n"}, "Migration name", typeof(string), true),
				CreateOption(new[] {"--path", "-p"}, "Path where migration file will be created", typeof(string), true)
			);

			command.AddCommand(subCommandNew);

			command.AddCommand(CreateCommand(
				"handshake",
				"Perform handshake connection to database",
				CommandHandler.Create<string>(_migrationOperations.HandshakeDatabaseConnectionAsync),
				CreateOption(new[] {"--config-path", "-c"}, "Custom path to configuration file", typeof(string), false)
			));

			command.AddCommand(CreateCommand(
				"run",
				"Apply migrations",
				CommandHandler.Create<string, string, bool, string, string, string, string>(_migrationOperations.RunMigrationsAsync),
				CreateOption(new[] {"--migrations-root-path", "-m"}, "Root path to database migration files", typeof(string), true),
				CreateOption(new[] {"--config-path", "-c"}, "Custom path to configuration file", typeof(string), false),
				CreateOption(new[] {"--silent", "-s"}, "Show less info about migration status", typeof(bool), false),
				CreateOption(new[] {"--connection-string"}, "Database connection string", typeof(string), false),
				CreateOption(new[] {"--provider-name"}, "Database provider name", typeof(string), false),
				CreateOption(new[] {"--migration-schema"}, "Database schema for you migrations", typeof(string), false),
				CreateOption(new[] {"--migration-table"}, "Database table, where all migrations are stored", typeof(string), false)
			));

			command.AddCommand(CreateCommand(
				"status",
				"Show required actions for migration files",
				CommandHandler.Create<string, string, bool>(_migrationOperations.ShowMigrationsStatusAsync),
				CreateOption(new[] {"--migrations-root-path", "-m"}, "Root path to database migration files", typeof(string), true),
				CreateOption(new[] {"--config-path", "-c"}, "Custom path to configuration file", typeof(string), false),
				CreateOption(new[] {"--silent", "-s"}, "Show less info about migration status", typeof(bool), false)
			));

			RootCommand.AddCommand(subCommandNew);
			RootCommand.AddCommand(command);
		}
	}
}
