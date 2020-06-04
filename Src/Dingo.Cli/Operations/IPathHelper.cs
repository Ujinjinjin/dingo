namespace Dingo.Cli.Operations
{
	internal interface IPathHelper
	{
		string BuildFilePath(string path, string filename, string extension);
		string GetApplicationBaseDirectory();
		string GetExecutionBaseDirectory();
		string GetAbsolutePathFromRelative(string relativePath);
	}
}
