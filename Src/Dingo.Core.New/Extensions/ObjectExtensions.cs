using Dingo.Core.Exceptions;

namespace Dingo.Core.Extensions;

internal static class ObjectExtensions
{
	public static T Required<T>(this T? argument, string? paramName)
	{
		return argument ?? throw new ValueRequiredException(paramName);
	}
}
