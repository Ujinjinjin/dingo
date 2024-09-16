using System.Data;
using Dapper;
using Dingo.Core.Models;
using Dingo.Core.Repository.Models;
using Microsoft.Data.SqlClient.Server;

namespace Dingo.Core.Repository.Command;

internal sealed class SqlServerCommandProvider : ICommandProvider
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
		IList<SqlDataRecord> records = new List<SqlDataRecord>();
		foreach (var item in migrationInfoInputs)
		{
			var record = new SqlDataRecord(
				new SqlMetaData("migration_hash", SqlDbType.VarChar, 256),
				new SqlMetaData("migration_path", SqlDbType.VarChar, 512)
			);
			record.SetString(0, item.MigrationHash);
			record.SetString(1, item.MigrationPath);

			records.Add(record);
		}

		return new Command(
			"dingo._get_migrations_status",
			new { pti_migration_info_input = records.AsTableValuedParameter() },
			CommandType.StoredProcedure
		);
	}

	public Command GetNextPatch(PatchType patchType)
	{
		return new Command(
			"dingo._next_patch",
			new { p_patch_type = patchType },
			CommandType.StoredProcedure
		);
	}

	public Command GetLastPatchMigrations(int patchCount)
	{
		return new Command(
			"dingo._get_last_patch",
			new { p_patch_count = patchCount },
			CommandType.StoredProcedure
		);
	}

	public Command RegisterMigration(Migration migration, int patchNumber)
	{
		return new Command(
			"dingo._register_migration",
			new
			{
				p_migration_path = migration.Path.Relative,
				p_migration_hash = migration.Hash.Value,
				p_patch_number = patchNumber,
			},
			CommandType.StoredProcedure
		);
	}

	public Command RevertPatch(int patchNumber)
	{
		return new Command(
			"dingo._revert_patch",
			new { p_patch_number = patchNumber },
			CommandType.StoredProcedure
		);
	}

	public Command CompletePatch(int patchNumber)
	{
		return new Command(
			"dingo._complete_patch",
			new { p_patch_number = patchNumber },
			CommandType.StoredProcedure
		);
	}
}
