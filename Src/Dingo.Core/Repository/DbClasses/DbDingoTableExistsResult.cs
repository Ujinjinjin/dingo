using JetBrains.Annotations;
using LinqToDB.Mapping;

namespace Dingo.Core.Repository.DbClasses
{
	[UsedImplicitly]
	internal class DbDingoTableExistsResult
	{
		[Column("dingo__table_exists")]
		public bool DingoTableExists { get; set; }	
	}
}