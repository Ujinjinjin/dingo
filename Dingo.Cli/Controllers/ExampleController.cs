using Dingo.Abstractions;
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
			Console.WriteLine($"Nested: {option}");
		}
		
		[SubCommand("embedded", Description = "Embedded command", StackType = StackType.Embedded)]
		[Option("Embedded command option", typeof(string), "--option", "-o")]
		public void EmbeddedCommand(string option)
		{
			Console.WriteLine($"Embedded: {option}");
		}
	}
}
