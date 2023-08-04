using Dingo.Core;
using Dingo.Core.Migrations;
using Dingo.Core.Validators.MigrationValidators;

namespace Dingo.UnitTests;

public class UnitTestBase
{
	protected readonly Fixture Fixture;

	protected UnitTestBase()
	{
		Fixture = new Fixture();
	}

	protected Migration CreateMigration(string up, string down)
	{
		return new Migration(up, down);
	}
}
