using Dingo.Cli.Repository;

namespace Dingo.Cli.Factories
{
	internal interface IDatabaseContextFactory
	{
		IDatabaseContext CreateDatabaseContext(string dbProviderName, string connectionString);
	}
}