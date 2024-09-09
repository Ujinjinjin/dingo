namespace Dingo.Core.Exceptions;

public class DingoException : ApplicationException
{
	public DingoException(string? message)
		: base(message)
	{
	}
}
