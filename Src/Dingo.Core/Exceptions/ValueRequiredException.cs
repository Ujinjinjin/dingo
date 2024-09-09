namespace Dingo.Core.Exceptions;

internal sealed class ValueRequiredException : DingoException
{
	private const string ErrorPrefix = "Missing required argument: {0}";

	public ValueRequiredException(string? paramName)
		: base(string.Format(ErrorPrefix, paramName))
	{
	}

	public ValueRequiredException(string message, string? paramName)
		: base($"{string.Format(ErrorPrefix, paramName)}. {message}")
	{
	}
}
