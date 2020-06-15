namespace Dingo.Core.Config
{
	public interface IConfiguration
	{
		string CheckTableExistenceProcedurePath { get; set; }
		string DingoMigrationsRootPath { get; set; }
		string MigrationsSearchPattern { get; set; }
		string ConnectionString { get; set; }
		string ProviderName { get; set; }
		string MigrationSchema { get; set; }
		string MigrationTable { get; set; }
	}
}
