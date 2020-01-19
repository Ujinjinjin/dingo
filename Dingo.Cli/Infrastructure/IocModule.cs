using Dingo.Abstractions;
using Dingo.Abstractions.Config;
using Dingo.Abstractions.Operations;
using Dingo.Cli.Controllers;
using Dingo.Core;
using Dingo.Core.Config;
using Dingo.Core.Factories;
using Dingo.Core.Operations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CommandLine;
using YamlDotNet.Serialization;

namespace Dingo.Cli.Infrastructure
{
	internal class IocModule : IIocModule
	{
		public IServiceProvider Build()
		{
			var collection = new ServiceCollection();

			var rootCommand = new RootCommand("start");
			collection.AddSingleton(rootCommand);
			
			// Services
			collection.AddSingleton<ICliService, CliService>();

			// Register controllers
			// collection.AddSingleton<IController, ExampleController>();
			collection.AddSingleton<IController, ConfigController>();
			
			// Operations
			collection.AddSingleton<IConfigOperations, ConfigOperations>();
			
			// Load configuration files
			LoadConfigs(collection);

			return collection.BuildServiceProvider();
		}

		private void LoadConfigs(IServiceCollection collection)
		{
			var deserializer = new Deserializer();
			var serializer = new Serializer();
			var configFactory = new ConfigFactory(deserializer, serializer);

			collection.AddSingleton<IDeserializer>(deserializer);
			collection.AddSingleton<ISerializer>(serializer);
			collection.AddSingleton(configFactory.LoadGlobalConfig());
			collection.AddSingleton(configFactory.LoadProjectConfig());
			collection.AddSingleton<IDingoConfig, DingoConfig>();
		}
	}
}
