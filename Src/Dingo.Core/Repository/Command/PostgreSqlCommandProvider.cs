using System.Data;
using Dapper;
using Dingo.Core.Models;
using Dingo.Core.Repository.Models;

namespace Dingo.Core.Repository.Command;

internal sealed class PostgreSqlCommandProvider : ICommandProvider
{
	public Command SelectSchema(string schema)
	{
		return new Command(
			"select schema_name from information_schema.schemata where schema_name = @SchemaName",
			new { SchemaName = schema },
			CommandType.Text
		);
	}

	public Command GetMigrationsStatus(IReadOnlyList<MigrationComparisonInput> migrationInfoInputs)
	{
		var parameters = new DynamicParameters();
		parameters.Add("pti_migration_info_input", migrationInfoInputs);

		return new Command(
			"select * from dingo._get_migrations_status(@PtiMigrationInfoInput)",
			new { PtiMigrationInfoInput = migrationInfoInputs },
			CommandType.Text
		);
	}

	public Command GetNextPatch()
	{
		return new Command(
			"select * from dingo._next_patch()",
			null,
			CommandType.Text
		);
	}

	public Command GetLastPatchMigrations(int patchCount)
	{
		return new Command(
			"select * from dingo._get_last_patch(@PatchCount)",
			new { PatchCount = patchCount },
			CommandType.Text
		);
	}

	public Command RegisterMigration(Migration migration, int patchNumber)
	{
		return new Command(
			"select * from dingo._register_migration(@MigrationPath, @MigrationHash, @PatchNumber)",
			new
			{
				MigrationPath = migration.Path.Relative,
				MigrationHash = migration.Hash.Value,
				PatchNumber = patchNumber,
			},
			CommandType.Text
		);
	}

	public Command RevertPatch(int patchNumber)
	{
		return new Command(
			"select * from dingo._revert_patch(@PatchNumber)",
			new { PatchNumber = patchNumber },
			CommandType.Text
		);
	}

	public Command CompletePatch(int patchNumber)
	{
		return new Command(
			"select * from dingo._complete_patch(@PatchNumber)",
			new { PatchNumber = patchNumber },
			CommandType.Text
		);
	}
}
