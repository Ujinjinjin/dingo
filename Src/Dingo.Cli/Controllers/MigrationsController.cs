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
			var command = CreateCommand("migrations", "Group of command to work with migrations");

			command.AddCommand(CreateCommand(
				"handshake",
				"perform handshake connection to database to validate connection string",
				CommandHandler.Create<string>(_migrationOperations.HandshakeDatabaseConnectionAsync),
				CreateOption(new[] {"--config-path", "-c"}, "Custom path to configuration file", typeof(string), false)
			));

			command.AddCommand(CreateCommand(
				"run",
				"run migrations",
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
				"show migration status",
				CommandHandler.Create<string, string, bool>(_migrationOperations.ShowMigrationsStatusAsync),
				CreateOption(new[] {"--migrations-root-path", "-m"}, "Root path to database migration files", typeof(string), true),
				CreateOption(new[] {"--config-path", "-c"}, "Custom path to configuration file", typeof(string), false),
				CreateOption(new[] {"--silent", "-s"}, "Show less info about migration status", typeof(bool), false)
			));

			RootCommand.AddCommand(command);
		}
	}
}
