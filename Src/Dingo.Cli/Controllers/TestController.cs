using System.CommandLine;
using Cliff;
using Cliff.Factories;

namespace Dingo.Cli.Controllers;

public class TestController : CliController
{
	public TestController(
		RootCommand rootCommand,
		ICommandFactory commandFactory,
		IOptionFactory optionFactory
	) : base(
		rootCommand,
		commandFactory,
		optionFactory
	)
	{
	}

	public override void Register()
	{
		var command = CommandFactory.CreateCommand("test", "Test command", Visibility.Hidden);

		var subcommandOutput = GetOutputCommand();

		command.Add(subcommandOutput);

		Register(command);
	}

	private Command GetOutputCommand()
	{
		var option = OptionFactory.CreateOption<string>(new[] { "--output", "-o" }, "Output value", true);

		var command = CommandFactory.CreateCommand("subcommand", "Test subcommand", Visibility.Hidden, option);

		command.SetHandler(Console.WriteLine, option);

		return command;
	}
}
