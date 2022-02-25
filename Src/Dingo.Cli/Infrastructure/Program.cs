using Cliff.Infrastructure;
using Dingo.Core.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.Cli.Infrastructure;

/// <summary> Program`s entry point class </summary>
internal static class Program
{
	/// <summary> Program`s entry point method </summary>
	/// <param name="args">Arguments passed to application</param>
	private static async Task Main(string[] args)
	{
		var serviceProvider = new IocModule()
			.Build();

		var dingo = serviceProvider.GetService<ICliService>();
		var outputQueues = serviceProvider
			.GetServices<IOutputQueue>()
			.ToArray();

		if (dingo is null)
		{
			throw new Exception($"Couldn't find any registered {nameof(ICliService)}");
		}

		await dingo.ExecuteAsync(args);

		do
		{
			await Task.Delay(100);
			// wait until queues are empty
		} while (!outputQueues.All(x => x.IsEmpty));
	}
}