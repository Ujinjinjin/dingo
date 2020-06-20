using Dingo.Core.Repository;

namespace Dingo.Core.Factories
{
	internal interface IDatabaseContextFactory
	{
		IDatabaseContext CreateDatabaseContext(string dbProviderName, string connectionString);
	}
}