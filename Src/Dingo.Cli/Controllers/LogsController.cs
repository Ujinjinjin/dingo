using Cliff;
using Dingo.Core.Operations;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Dingo.Cli.Controllers
{
	/// <summary> Controller allowing to work with logs </summary>
	internal class LogsController : CliController
	{
		private readonly ILogsOperations _logsOperations;

		public LogsController(RootCommand rootCommand, ILogsOperations logsOperations) : base(rootCommand)
		{
			_logsOperations = logsOperations ?? throw new ArgumentNullException(nameof(logsOperations));
		}

		public override void Register()
		{
			var command = CreateCommand("logs", "Group of command to work with logs");

			command.AddCommand(CreateCommand(
				"prune", 
				"Prune dingo log files",
				CommandHandler.Create(_logsOperations.PruneLogsAsync)
			));

			RootCommand.AddCommand(command);
		}
	}
}
