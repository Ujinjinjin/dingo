namespace Dingo.Core.Config
{
	/// <summary> Project level configurations </summary>
	internal struct ProjectConfiguration : IConfiguration
	{
		public string TableExistsProcedurePath { get; set; }
		public string DingoMigrationsRootPath { get; set; }
		public string MigrationsSearchPattern { get; set; }
		public string ConnectionString { get; set; }
		public string ProviderName { get; set; }
		public string MigrationSchema { get; set; }
		public string MigrationTable { get; set; }
		public int? LogLevel { get; set; }
	}
}