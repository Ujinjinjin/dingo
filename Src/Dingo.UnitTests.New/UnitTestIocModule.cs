using Dingo.Core.New.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.UnitTests.New;

public class UnitTestIocModule
{
	public IServiceProvider Build()
	{
		ServiceCollection serviceCollection = new ServiceCollection();

		serviceCollection.UseDingo();

		return serviceCollection.BuildServiceProvider();
	}
}
