using Dingo.Core.Models;
using Microsoft.Extensions.Logging;

namespace Dingo.UnitTests;

public class UnitTestBase
{
	protected readonly Fixture Fixture;

	protected UnitTestBase()
	{
		Fixture = new Fixture();
	}

	protected Migration CreateMigration(MigrationPath path, Hash hash, MigrationCommand command)
	{
		return new Migration(path, hash, command);
	}

	protected ILoggerFactory SetupLoggerFactory()
	{
		var factory = new Mock<ILoggerFactory>();

		return factory.Object;
	}
}
