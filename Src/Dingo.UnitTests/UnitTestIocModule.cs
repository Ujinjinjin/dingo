using Dingo.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.UnitTests;

public class UnitTestIocModule
{
	public IServiceProvider Build()
	{
		ServiceCollection serviceCollection = new ServiceCollection();

		serviceCollection.AddDingo();

		return serviceCollection.BuildServiceProvider();
	}
}
