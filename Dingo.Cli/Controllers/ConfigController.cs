using Dingo.Abstractions;
using Dingo.Core.Attributes;
using System;

namespace Dingo.Cli.Controllers
{
	[SubCommand("config")]
	internal class ConfigController : IController
	{
		[SubCommand("doSomeStuff")]
		[Option("shit", typeof(int), "--shit", "-s")]
		public void DoSomeStuff(int shit)
		{
			Console.WriteLine($"Shit: {shit}");
		}
	}
}
