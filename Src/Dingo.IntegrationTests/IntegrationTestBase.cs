using AutoFixture;
using Dingo.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.IntegrationTests;

public class IntegrationTestBase
{
	protected readonly Fixture Fixture;
	protected readonly IServiceProvider ServiceProvider;

	protected IntegrationTestBase()
	{
		Fixture = new Fixture();
		ServiceProvider = BuildServiceProvider();

	}

	private IServiceProvider BuildServiceProvider()
	{
		var sc = new ServiceCollection();
		sc.AddDingo();
		var sp = sc.BuildServiceProvider();
		sp.UseDingo();

		return sp;
	}
}
