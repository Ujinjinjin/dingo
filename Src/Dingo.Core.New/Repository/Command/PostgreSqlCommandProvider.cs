using System.Data;
using Dapper;
using Dingo.Core.Repository.Models;

namespace Dingo.Core.Repository.Command;

public class PostgreSqlCommandProvider : ICommandProvider
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
			"select * from dingo._get_migrations_status(@pti_migration_info_input)",
			parameters,
			CommandType.Text
		);
	}
}
