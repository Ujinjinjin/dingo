using Dingo.Core.Repository.DbClasses;
using LinqToDB.Data;

namespace Dingo.Core.Repository.DbConverters;

/// <summary> Database contract converter </summary>
internal interface IDatabaseContractConverter
{
	/// <summary> Convert list of <see cref="DbMigrationInfoInput"/> to data parameter </summary>
	DataParameter ToDataParameter(string parameterName, IList<DbMigrationInfoInput> source);
}