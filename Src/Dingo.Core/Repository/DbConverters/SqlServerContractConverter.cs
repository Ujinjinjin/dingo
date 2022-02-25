using Dingo.Core.Repository.DbClasses;
using LinqToDB.Data;

namespace Dingo.Core.Repository.DbConverters;

/// <summary> Microsoft Sql Server database contract converter </summary>
internal sealed class SqlServerContractConverter : DatabaseContractConverterBase, IDatabaseContractConverter
{
	/// <inheritdoc />
	public DataParameter ToDataParameter(string parameterName, IList<DbMigrationInfoInput> source)
	{
		var rowMapList = new[]
		{
			new RowMap<DbMigrationInfoInput>("migration_path", x => x.MigrationPath),
			new RowMap<DbMigrationInfoInput>("migration_hash", x => x.MigrationHash),
		};

		return ToDataParameter(parameterName, "t_migration_info_input", source, rowMapList);
	}
}