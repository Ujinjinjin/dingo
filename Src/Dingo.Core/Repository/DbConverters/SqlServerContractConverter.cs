using Dingo.Core.Repository.DbClasses;
using LinqToDB.Data;
using System.Collections.Generic;

namespace Dingo.Core.Repository.DbConverters
{
	/// <summary> Microsoft Sql Server database contract converter </summary>
	internal class SqlServerContractConverter : DatabaseContractConverterBase, IDatabaseContractConverter
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
}
