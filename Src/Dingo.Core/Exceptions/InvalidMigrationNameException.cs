namespace Dingo.Core.Exceptions;

internal class InvalidMigrationNameException : DingoException
{
	private const string ErrorPrefix = "Migration name is invalid";

	public InvalidMigrationNameException()
		: base(ErrorPrefix)
	{
	}

	public InvalidMigrationNameException(string message)
		: base($"{ErrorPrefix}. {message}")
	{
	}
}
