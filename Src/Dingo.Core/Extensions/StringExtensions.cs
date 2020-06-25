namespace Dingo.Core.Extensions
{
	internal static class StringExtensions
	{
		public static string ReplaceBackslashesWithSlashes(this string value)
		{
			return value.Replace("\\", "/");
		}

		public static bool NotContains(this string source, string value)
		{
			return !source.Contains(value);
		}

		public static string ToUnixEol(this string source)
		{
			return source.Replace("\r\n", "\n");
		}
	}
}
