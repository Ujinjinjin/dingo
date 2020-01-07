using Dingo.Abstractions;
using Dingo.Cli.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CommandLine;

namespace Dingo.Cli.Infrastructure
{
	internal class Container : IContainer
	{
		public IServiceProvider Build()
		{
			var collection = new ServiceCollection();
			
			var rootCommand = new RootCommand("start");
			collection.AddSingleton(rootCommand);
			
			// Services
			collection.AddSingleton<ICliService, CliService>();

			// Register controllers
			collection.AddSingleton<IController, ConfigController>();
			// collection.AddSingleton<IController, ActionController>();

			return collection.BuildServiceProvider();
		}
	}
}
