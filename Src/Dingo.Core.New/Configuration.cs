namespace Dingo.Core;

internal static class Configuration
{
	internal static class Key
	{
		public const string ConnectionString = "connection-string";
		public const string MigrationDelimiter = "migration-delimiter";
		public const string MigrationWildcard = "migration-wildcard";
	}

	public static readonly IDictionary<string, string> Dict = new Dictionary<string, string>
	{
		{Key.ConnectionString, ""},
		{Key.MigrationDelimiter, @"^--\s*down$"},
		{Key.MigrationWildcard, "*.sql"},
	};
}
