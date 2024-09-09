namespace Dingo.Core.Exceptions;

internal class UnitOfWorkNotDefinedException : DingoException
{
	private const string ErrorMessage = "No active unit of work was found. Units of work must be managed at application level";

	public UnitOfWorkNotDefinedException()
		: base(ErrorMessage)
	{
	}
}
