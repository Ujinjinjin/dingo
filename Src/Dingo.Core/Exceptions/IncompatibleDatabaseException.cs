namespace Dingo.Core.Exceptions;

internal class IncompatibleDatabaseException : DingoException
{
	private const string ErrorPrefix = "Current database {0} is not compatible with requested operation";

	public IncompatibleDatabaseException(string providerName)
		: base(string.Format(ErrorPrefix, providerName))
	{
	}

	public IncompatibleDatabaseException(string providerName, string message)
		: base($"{string.Format(ErrorPrefix, providerName)}. {message}")
	{
	}
}
