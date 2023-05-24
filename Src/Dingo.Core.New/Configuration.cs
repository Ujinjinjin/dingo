namespace Dingo.Core;

internal static class Configuration
{
	public static readonly IDictionary<string, string> Keys = new Dictionary<string, string>
	{
		{"connection-string", ""},
		{"migration-delimiter", @"^--down$"},
	};
}
