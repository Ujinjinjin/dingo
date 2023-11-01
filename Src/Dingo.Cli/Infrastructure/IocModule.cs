using Cliff;
using Cliff.Infrastructure;
using Dingo.Cli.Controllers;
using Dingo.Cli.Implementors;
using Dingo.Core.Extensions;
using Dingo.Core.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.Cli.Infrastructure;

/// <inheritdoc />
public sealed class IocModule : BaseIocModule
{
	public IocModule() : base(
		"dingo",
		"dingo is framework-agnostic and lightweight database migration tool"
	)
	{
	}

	protected override void RegisterServices(IServiceCollection collection)
	{
		collection.AddDingo();

		collection.AddSingleton<IOutput, CliOutput>();

		collection.AddSingleton<IController, MigrationController>();
		collection.AddSingleton<IController, ConfigController>();
		collection.AddSingleton<IController, ConnectionController>();
		collection.AddSingleton<IController, LogsController>();
		collection.AddSingleton<IController, TestController>();
	}
}
