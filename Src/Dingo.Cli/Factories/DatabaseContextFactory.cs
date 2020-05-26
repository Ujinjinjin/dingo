using Dingo.Cli.Repository;
using LinqToDB;
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
					return new PostgresDbContext(connectionString);
				default:
					throw new ArgumentOutOfRangeException(dbProviderName);
			}
		}
	}
}