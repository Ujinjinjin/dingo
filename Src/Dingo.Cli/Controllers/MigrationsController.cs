using Cliff;
using Dingo.Core.Operations;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Dingo.Cli.Controllers
{
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

		public override void Register()
		{
			var command = CreateCommand("migrations", "Group of command to work with migrations");
			
			command.AddCommand(CreateCommand(
				"status",
				"show migration status",
				CommandHandler.Create<string, string, bool>(_migrationOperations.ShowMigrationsStatusAsync),
				CreateOption(new[] {"--migrationsRootPath", "-m"}, "Root path to database migration files", typeof(string), true),
				CreateOption(new[] {"--configPath", "-c"}, "Custom path to configuration file", typeof(string), false),
				CreateOption(new[] {"--silent", "-s"}, "Show less info about migration status", typeof(bool), false)
			));
			
			RootCommand.AddCommand(command);
		}
	}
}
