using Cliff;
using System;
using System.CommandLine;

namespace Dingo.Cli.Controllers
{
	internal class ConfigController : IController
	{
		private readonly RootCommand _rootCommand;
		
		public ConfigController(RootCommand rootCommand)
		{
			_rootCommand = rootCommand ?? throw new ArgumentNullException(nameof(rootCommand));
		}
		
		public void Register()
		{
			var command = new Command("heck", "snek");
			_rootCommand.AddCommand(command);
		}
	}
}