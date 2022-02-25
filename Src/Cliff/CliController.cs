using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Cliff;

/// <summary> Base CLI controller </summary>
public abstract class CliController : IController
{
	/// <summary> Root cli command </summary>
	protected readonly RootCommand RootCommand;

	protected CliController(RootCommand rootCommand)
	{
		RootCommand = rootCommand ?? throw new ArgumentNullException(nameof(rootCommand));
	}

	/// <inheritdoc />
	public abstract void Register();

	/// <summary> Create CLI command </summary>
	/// <param name="name">Command name</param>
	/// <param name="description">Command description</param>
	/// <param name="options">List of options</param>
	/// <returns><see cref="Command"/></returns>
	protected Command CreateCommand(string name, string description, params Option[] options)
	{
		return CreateCommand(name, description, null, options);
	}

	/// <summary> Create CLI command </summary>
	/// <param name="name">Command name</param>
	/// <param name="description">Command description</param>
	/// <param name="commandHandler">Command handler</param>
	/// <param name="options">List of options</param>
	/// <returns><see cref="Command"/></returns>
	protected Command CreateCommand(string name, string description, ICommandHandler commandHandler, params Option[] options)
	{
		var command = new Command(name, description);

		for (var i = 0; i < options.Length; i++)
		{
			command.AddOption(options[i]);
		}

		command.Handler = commandHandler;

		return command;
	}

	/// <summary> Create command option </summary>
	/// <param name="aliases">List of option's aliases</param>
	/// <param name="description">Option description</param>
	/// <param name="type">Option type</param>
	/// <param name="required">Defines if option is required or not</param>
	/// <returns><see cref="Option"/></returns>
	protected Option CreateOption(string[] aliases, string description, Type type, bool required)
	{
		return new(aliases, description)
		{
			Argument = new Argument { ArgumentType = type },
			Required = required,
		};
	}
}