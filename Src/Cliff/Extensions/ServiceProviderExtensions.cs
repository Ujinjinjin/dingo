using Microsoft.Extensions.DependencyInjection;

namespace Cliff.Extensions;

/// <summary> Collection of extensions for <see cref="IServiceProvider"/></summary>
public static class ServiceProviderExtensions
{
	/// <summary> Invoke <see cref="CliController.Register"/> method for every controller in service collection </summary>
	/// <param name="serviceProvider">Service provider of your application</param>
	/// <returns>Self</returns>
	public static IServiceProvider RegisterControllers(this IServiceProvider serviceProvider)
	{
		var controllers = serviceProvider.GetServices<IController>();
		foreach (var controller in controllers)
		{
			controller.Register();
		}

		return serviceProvider;
	}
}