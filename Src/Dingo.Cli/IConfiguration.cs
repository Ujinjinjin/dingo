namespace Dingo.Cli
{
	internal interface IConfiguration
	{
		string CheckTableExistenceProcedureFilePath { get; }
		string DingoDatabaseScriptsMask { get; }
	}
}
