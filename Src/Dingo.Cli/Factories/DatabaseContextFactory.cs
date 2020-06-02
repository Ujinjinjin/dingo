using Dingo.Cli.Repository;
using Dingo.Cli.Repository.DbClasses;
using LinqToDB;
using Npgsql;
using System;

namespace Dingo.Cli.Factories
{
	internal class DatabaseContextFactory : IDatabaseContextFactory
	{
		public IDatabaseContext CreateDatabaseContext(string dbProviderName, string connectionString)
		{
			switch (dbProviderName)
			{
				case ProviderName.PostgreSQL95:
					NpgsqlConnection.GlobalTypeMapper.MapComposite<DbMigrationInfoInput>("t_migration_info_input");
					return new DatabaseContext(dbProviderName, connectionString);
				default:
					throw new ArgumentOutOfRangeException(dbProviderName);
			}
		}
	}
}