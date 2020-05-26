namespace Dingo.Cli
{
	internal class PostgresConfiguration : IConfiguration
	{
		public string CheckTableExistenceProcedureFilePath => "Database/PostreSQL/procedures/system__check_table_existence.sql";
		public string DingoDatabaseScriptsMask => "Database/PostreSQL/**.sql";
	}
}
