namespace Dingo.Core.Exceptions;

internal class DingoException : ApplicationException
{
	protected DingoException(string? message)
		: base(message)
	{
	}
}
