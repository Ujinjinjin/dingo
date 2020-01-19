using Dingo.Abstractions;
using Dingo.Abstractions.Infrastructure;
using Dingo.Core.Attributes;
using System;

namespace Dingo.Cli.Controllers
{
	[SubCommand("example", "Example command group (aka controller)")]
	internal class ExampleController : IController
	{
		[SubCommand("nested", Description = "Nested command")]
		[Option("Nested command option", typeof(int), "--option")]
		public void NestedCommand(int option)
		{
			Console.WriteLine($"Nested: {option.ToString()}");
		}
		
		[SubCommand("embedded", Description = "Embedded command", StackType = StackType.Embedded)]
		[Option("Embedded required command option", typeof(string), "--requiredOption", "-r")]
		[Option("Embedded optional command option", typeof(string), false, "--optionalOption", "-o")]
		public void EmbeddedCommand(string requiredOption, string optionalOption)
		{
			Console.WriteLine($"Embedded: {requiredOption}");
		}
	}
}
