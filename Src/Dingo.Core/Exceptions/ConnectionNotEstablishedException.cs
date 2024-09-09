namespace Dingo.Core.Exceptions;

internal sealed class ConnectionNotEstablishedException : DingoException
{
	private const string ErrorPrefix = "Database connection was not established, please check if conection string was set correctly";

	public ConnectionNotEstablishedException()
		: base(ErrorPrefix)
	{
	}

	public ConnectionNotEstablishedException(string message)
		: base($"{ErrorPrefix}. {message}")
	{
	}
}
