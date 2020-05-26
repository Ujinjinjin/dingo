namespace Dingo.Cli
{
	internal class PostgresConfiguration : IConfiguration
	{
		public string CheckTableExistenceProcedureFilePath => "Database/PostreSQL/procedures/system__check_table_existence.sql";
		public string DingoDatabaseScriptsMask => "Database/PostreSQL/**.sql";
		public string ConnectionString => "Server=localhost;Port=5432;Database=dingo_db;User Id=local_user;Password=qwer1234;";
		public string ProviderName => LinqToDB.ProviderName.PostgreSQL95;
		public string MigrationSchema => "public";
		public string MigrationTable => "dingo_migration";
	}
}
