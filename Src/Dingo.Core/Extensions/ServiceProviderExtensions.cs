using Trico.Extensions;

namespace Dingo.Core.Extensions;

public static class ServiceProviderExtensions
{
	public static IServiceProvider UseDingo(this IServiceProvider sp)
	{
		return sp.UseConfiguration();
	}
}
