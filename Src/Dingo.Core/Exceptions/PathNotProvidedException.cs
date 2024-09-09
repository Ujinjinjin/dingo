namespace Dingo.Core.Exceptions;

internal sealed class PathNotProvidedException : DingoException
{
	private const string ErrorMessage = "Migrations path not provided. Either set it in the profile or with '-p' parameter. Use '--help' for more information.";

	public PathNotProvidedException()
		: base(ErrorMessage)
	{
	}
}
