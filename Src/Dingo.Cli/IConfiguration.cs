namespace Dingo.Cli
{
	internal interface IConfiguration
	{
		string CheckTableExistenceProcedureFilePath { get; }
		string DingoDatabaseScriptsMask { get; }
		string ConnectionString { get; }
		string ProviderName { get; }
		string MigrationSchema { get; }
		string MigrationTable { get; }
	}
}
