using Cliff.ConsoleUtils;
using Cliff.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Cliff.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddCliff(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<ICliService, CliService>();
			serviceCollection.AddSingleton<IConsoleQueue, ConsoleQueue>();
		}
	}
}