namespace Dingo.Core;

internal static class Configuration
{
	internal static class Key
	{
		public const string ConnectionString = "connection-string";
		public const string MigrationDelimiter = "migration-delimiter";
	}

	public static readonly IDictionary<string, string> Dict = new Dictionary<string, string>
	{
		{Key.ConnectionString, ""},
		{Key.MigrationDelimiter, @"^--down$"},
	};
}
