using Cliff;
using Cliff.Extensions;
using Cliff.Infrastructure;
using Dingo.Cli.Controllers;
using Dingo.Core.Extensions;
using Dingo.Core.Renderer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CommandLine;

namespace Dingo.Cli
{
	public class IocModule : IIocModule
	{
		public IServiceProvider Build()
		{
			var collection = new ServiceCollection();

			var rootCommand = new RootCommand("dingo is incremental databse installator") { Name = "dingo" };
			collection.AddSingleton(rootCommand);
			
			collection.UseDingo();
			collection.UseCliff();
			
			collection.AddSingleton<IRenderer, CliRenderer>();
			
			collection.AddSingleton<IController, ConfigController>();
			
			return collection.BuildServiceProvider();
		}
	}
}