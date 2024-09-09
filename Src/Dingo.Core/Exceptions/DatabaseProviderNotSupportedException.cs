namespace Dingo.Core.Exceptions;

internal sealed class DatabaseProviderNotSupportedException : DingoException
{
	private const string ErrorPrefix = "Database provider {0} not supported";

	public DatabaseProviderNotSupportedException(string providerName)
		: base(string.Format(ErrorPrefix, providerName))
	{
	}

	public DatabaseProviderNotSupportedException(string providerName, string message)
		: base($"{string.Format(ErrorPrefix, providerName)}. {message}")
	{
	}
}
