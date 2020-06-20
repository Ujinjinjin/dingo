using Cliff.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Cliff.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void UseCliff(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<ICliService, CliService>();
		}
	}
}