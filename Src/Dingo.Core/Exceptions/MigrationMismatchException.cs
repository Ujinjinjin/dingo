namespace Dingo.Core.Exceptions;

internal class MigrationMismatchException : DingoException
{
	private const string ErrorPrefix = "Migration mismatch. Param: {0}";
	public readonly string ParamName;

	public MigrationMismatchException(string paramName)
		: base(string.Format(ErrorPrefix, paramName))
	{
		ParamName = paramName;
	}

	public MigrationMismatchException(string paramName, string message)
		: base($"{string.Format(ErrorPrefix, paramName)}. {message}")
	{
		ParamName = paramName;
	}
}
