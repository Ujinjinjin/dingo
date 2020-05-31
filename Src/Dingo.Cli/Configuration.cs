namespace Dingo.Cli
{
	internal class PostgresConfiguration : IConfiguration
	{
		public string CheckTableExistenceProcedurePath => $"Database/{ProviderName}/procedures/system__check_table_existence.sql";
		public string DingoMigrationsRootPath => $"Database/{ProviderName}/";
		public string MigrationsSearchPattern => "*.sql";
		public string ConnectionString => "Server=172.18.211.136;Port=5432;Database=dingo_db;User Id=local_user;Password=qwer1234;";
		public string ProviderName => LinqToDB.ProviderName.PostgreSQL95;
		public string MigrationSchema => "public";
		public string MigrationTable => "dingo_migration";
	}
}
