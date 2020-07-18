using Cliff.ConsoleUtils;
using Cliff.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Cliff.Extensions
{
	/// <summary> Collection of extensions for <see cref="IServiceCollection"/> </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary> Add cliff dependencies to service collection </summary>
		/// <param name="serviceCollection">Target service collection</param>
		public static void UseCliff(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<ICliService, CliService>();
			serviceCollection.AddSingleton<IConsoleQueue, ConsoleQueue>();
		}
	}
}