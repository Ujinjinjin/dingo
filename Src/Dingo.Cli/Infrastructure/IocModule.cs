using Cliff;
using Cliff.Infrastructure;
using Dingo.Cli.Controllers;
using Dingo.Cli.Implementors;
using Dingo.Core.Abstractions;
using Dingo.Core.Extensions;
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
		collection.UseDingo();

		collection.AddSingleton<IRenderer, CliRenderer>();
		collection.AddSingleton<IPrompt, CliPrompt>();

		collection.AddSingleton<IController, ConfigController>();
		collection.AddSingleton<IController, LogsController>();
		collection.AddSingleton<IController, MigrationsController>();
		collection.AddSingleton<IController, ProviderController>();
		collection.AddSingleton<IController, TestController>();
	}
}