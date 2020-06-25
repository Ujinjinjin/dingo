using System;
using System.CommandLine;

namespace Cliff
{
	public abstract class CliController : IController
	{
		protected readonly RootCommand RootCommand;
		
		protected CliController(RootCommand rootCommand)
		{
			RootCommand = rootCommand ?? throw new ArgumentNullException(nameof(rootCommand));
		}

		public abstract void Register();
		
		protected Command CreateCommand(string name, string description, params Option[] options)
		{
			var command = new Command(name, description);

			for (var i = 0; i < options.Length; i++)
			{
				command.AddOption(options[i]);
			}

			return command;
		}

		protected Option CreateOption(string[] aliases, string description, Type type, bool required)
		{
			return new Option(aliases, description)
			{
				Argument = new Argument { ArgumentType = type },
				Required = required
			};
		}
	}
}