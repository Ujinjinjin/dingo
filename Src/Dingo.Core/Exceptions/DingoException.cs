namespace Dingo.Core.Exceptions;

public class DingoException : ApplicationException
{
	protected DingoException(string? message)
		: base(message)
	{
	}
}
