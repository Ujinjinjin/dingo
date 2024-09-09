using Cliff.Infrastructure;
using Dingo.Core.Exceptions;
using Dingo.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.Cli.Infrastructure;

/// <summary> Program`s entry point class </summary>
internal static class Program
{
	/// <summary> Program`s entry point method </summary>
	/// <param name="args">Arguments passed to application</param>
	internal static async Task Main(string[] args)
	{
		var serviceProvider = new IocModule()
			.Build();
		serviceProvider.UseDingo();

		var dingo = serviceProvider.GetService<ICliService>();
		if (dingo is null)
		{
			throw new DingoException($"Couldn't find any registered {nameof(ICliService)}");
		}

		await dingo.ExecuteAsync(args);
	}
}
