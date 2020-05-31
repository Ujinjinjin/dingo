namespace Dingo.Cli
{
	internal interface IConfiguration
	{
		string CheckTableExistenceProcedurePath { get; }
		string DingoMigrationsRootPath { get; }
		string MigrationsSearchPattern { get; }
		string ConnectionString { get; }
		string ProviderName { get; }
		string MigrationSchema { get; }
		string MigrationTable { get; }
	}
}
