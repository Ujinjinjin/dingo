namespace Dingo.Core.Config
{
	internal struct ProjectConfiguration : IConfiguration
	{
		public string CheckTableExistenceProcedurePath { get; set; }
		public string DingoMigrationsRootPath { get; set; }
		public string MigrationsSearchPattern { get; set; }
		public string ConnectionString { get; set; }
		public string ProviderName { get; set; }
		public string MigrationSchema { get; set; }
		public string MigrationTable { get; set; }
	}
}