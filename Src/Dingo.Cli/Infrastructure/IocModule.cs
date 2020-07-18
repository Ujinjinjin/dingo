using Cliff;
using Cliff.Extensions;
using Cliff.Infrastructure;
using Dingo.Cli.Controllers;
using Dingo.Cli.Implementors;
using Dingo.Core.Abstractions;
using Dingo.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CommandLine;

namespace Dingo.Cli.Infrastructure
{
	/// <inheritdoc />
	public class IocModule : IIocModule
	{
		/// <inheritdoc />
		public IServiceProvider Build()
		{
			var collection = new ServiceCollection();

			var rootCommand = new RootCommand("dingo is incremental database installer") { Name = "dingo" };
			collection.AddSingleton(rootCommand);
			
			collection.AddDingo();
			collection.UseCliff();
			
			collection.AddSingleton<IRenderer, CliRenderer>();
			collection.AddSingleton<IPrompt, CliPrompt>();
			
			collection.AddSingleton<IController, ConfigController>();
			collection.AddSingleton<IController, MigrationsController>();
			collection.AddSingleton<IController, ProviderController>();
			
			return collection.BuildServiceProvider();
		}
	}
}