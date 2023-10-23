using System.Data;
using Dingo.Core.Models;
using Dingo.Core.Repository.Models;

namespace Dingo.Core.Repository.Command;

public class SqlServerCommandProvider : ICommandProvider
{
	public Command SelectSchema(string schema)
	{
		return new Command(
			"select * from sys.schemas where name = @SchemaName",
			new { SchemaName = schema },
			CommandType: CommandType.Text
		);
	}

	public Command GetMigrationsStatus(IReadOnlyList<MigrationComparisonInput> migrationInfoInputs)
	{
		throw new NotImplementedException();
	}

	public Command GetNextPatch()
	{
		throw new NotImplementedException();
	}

	public Command GetLastPatchMigrations(int patchCount)
	{
		throw new NotImplementedException();
	}

	public Command RegisterMigration(Migration migration, int patchNumber)
	{
		throw new NotImplementedException();
	}

	public Command RevertPatch(int patchNumber)
	{
		throw new NotImplementedException();
	}

	public Command CompletePatch(int patchNumber)
	{
		throw new NotImplementedException();
	}
}
