using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cliff.Extensions
{
	public static class ServiceProviderExtensions
	{
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
}