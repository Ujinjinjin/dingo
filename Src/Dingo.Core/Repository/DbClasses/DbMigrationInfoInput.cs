using JetBrains.Annotations;
using LinqToDB.Mapping;

namespace Dingo.Core.Repository.DbClasses
{
	[UsedImplicitly]
	internal class DbMigrationInfoInput
	{
		[Column("migration_path")]
		public string MigrationPath { get; set; }

		[Column("migration_hash")]
		public string MigrationHash { get; set; }
	}
}