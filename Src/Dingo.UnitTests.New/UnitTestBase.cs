using Dingo.Core;
using Dingo.Core.Validators.MigrationValidators;

namespace Dingo.UnitTests.New;

public class UnitTestBase
{
	protected readonly Fixture Fixture;

	public UnitTestBase()
	{
		Fixture = new Fixture();
	}

	protected Migration CreateMigration(string up, string down)
	{
		return new Migration(up, down, Mock.Of<IMigrationValidator>());
	}
}
