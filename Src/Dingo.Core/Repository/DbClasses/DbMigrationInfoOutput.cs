using JetBrains.Annotations;
using LinqToDB.Mapping;
using System;

namespace Dingo.Core.Repository.DbClasses
{
	[UsedImplicitly]
	internal sealed class DbMigrationInfoOutput
	{
		[Column("migration_path")]
		public string MigrationPath { get; set; }

		[Column("new_hash")]
		public string NewHash { get; set; }

		[Column("old_hash")]
		public string OldHash { get; set; }

		[Column("is_outdated")]
		public bool? IsOutdated { get; set; }

		[Column("date_updated")]
		public DateTime? DateUpdated { get; set; }
	}
}