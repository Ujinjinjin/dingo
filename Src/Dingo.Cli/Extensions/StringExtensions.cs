namespace Dingo.Cli.Extensions
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
	}
}
