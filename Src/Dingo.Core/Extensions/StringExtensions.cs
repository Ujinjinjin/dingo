namespace Dingo.Core.Extensions;

/// <summary> Collection of extensions for <see cref="string"/> </summary>
internal static class StringExtensions
{
	/// <summary> Returns a concatenation of two path strings </summary>
	/// <param name="source">First path of path</param>
	/// <param name="value">Second part of path</param>
	public static string ConcatPath(this string source, string value)
	{
		var path = source + "/" + value;

		path = path.ReplaceBackslashesWithSlashes();

		while (path.Contains("//"))
		{
			path = path.Replace("//", "/");
		}

		return path;
	}
		
	/// <summary> Returns a value indicating whether a specified substring does not occur within this string </summary>
	/// <param name="source">Source string</param>
	/// <param name="value">Substring to search</param>
	public static bool NotContains(this string source, string value)
	{
		return !source.Contains(value);
	}
		
	/// <summary> Replace all `\` with `/` in given string </summary>
	/// <param name="value">Source string</param>
	public static string ReplaceBackslashesWithSlashes(this string value)
	{
		return value.Replace("\\", "/");
	}

	/// <summary> Replace all `\r\n` with `\n` in given string </summary>
	/// <param name="source">Source string</param>
	public static string ToUnixEol(this string source)
	{
		return source.Replace("\r\n", "\n");
	}
}