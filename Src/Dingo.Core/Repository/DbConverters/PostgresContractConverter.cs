using Dingo.Core.Repository.DbClasses;
using LinqToDB.Data;
using Npgsql;

namespace Dingo.Core.Repository.DbConverters;

/// <summary> PostgreSQL database contract converter </summary>
internal sealed class PostgresContractConverter : DatabaseContractConverterBase, IDatabaseContractConverter
{
	/// <summary> Public constructor </summary>
	public PostgresContractConverter()
	{
		NpgsqlConnection.GlobalTypeMapper.MapComposite<DbMigrationInfoInput>("t_migration_info_input");
	}
		
	/// <inheritdoc />
	public DataParameter ToDataParameter(string parameterName, IList<DbMigrationInfoInput> source)
	{
		return new(parameterName, source);
	}
}