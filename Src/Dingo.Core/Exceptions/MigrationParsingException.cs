namespace Dingo.Core.Exceptions;

internal class MigrationParsingException : DingoException
{
	private const string ErrorPrefix = "Migration file has invalid format and can't be parsed";

	public MigrationParsingException()
		: base(ErrorPrefix)
	{
	}

	public MigrationParsingException(string message)
		: base($"{ErrorPrefix}. {message}")
	{
	}
}
