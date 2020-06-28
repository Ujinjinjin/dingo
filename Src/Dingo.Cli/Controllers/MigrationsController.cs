using Cliff;
using Dingo.Core.Operations;
using System;
using System.CommandLine;

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
				"show migration status"
			));
			
			RootCommand.AddCommand(command);
		}
	}
}
