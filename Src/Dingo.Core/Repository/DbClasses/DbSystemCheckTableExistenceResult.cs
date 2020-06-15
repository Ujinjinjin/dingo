using JetBrains.Annotations;
using LinqToDB.Mapping;

namespace Dingo.Core.Repository.DbClasses
{
	[UsedImplicitly]
	internal class DbSystemCheckTableExistenceResult
	{
		[Column("system__check_table_existence")]
		public bool SystemCheckTableExistence { get; set; }	
	}
}