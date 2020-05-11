namespace Dingo.Cli.Operations
{
	internal interface IPathHelper
	{
		string GetApplicationBasePath();
		string GetAbsolutePathFromRelative(string relativePath);
	}
}
