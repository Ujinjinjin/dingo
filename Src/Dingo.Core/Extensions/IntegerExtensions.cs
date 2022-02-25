namespace Dingo.Core.Extensions;

/// <summary> Collection of extensions for <see cref="int"/> </summary>
internal static class IntegerExtensions
{
	/// <summary> Negate given number </summary>
	/// <param name="source"><see cref="int"/> number</param>
	/// <returns>Negated number</returns>
	public static int Negate(this int source)
	{
		return -1 * source;
	}
}