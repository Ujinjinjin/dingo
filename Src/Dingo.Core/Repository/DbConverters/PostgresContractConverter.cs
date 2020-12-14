using Dingo.Core.Repository.DbClasses;
using LinqToDB.Data;
using Npgsql;
using System.Collections.Generic;

namespace Dingo.Core.Repository.DbConverters
{
	/// <summary> PostgreSQL database contract converter </summary>
	internal class PostgresContractConverter : DatabaseContractConverterBase, IDatabaseContractConverter
	{
		/// <inheritdoc />
		public DataParameter ToDataParameter(string parameterName, IList<DbMigrationInfoInput> source)
		{
			NpgsqlConnection.GlobalTypeMapper.MapComposite<DbMigrationInfoInput>("t_migration_info_input");
			return new DataParameter(parameterName, source);
		}
	}
}
