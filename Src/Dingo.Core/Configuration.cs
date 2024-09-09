using Dingo.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Dingo.Core;

internal static class Configuration
{
	internal static class Key
	{
		public const string ConnectionString = "db.connection-string";
		public const string DatabaseProvider = "db.provider";
		public const string SchemaName = "db.schema";
		public const string MigrationDelimiter = "migration.delimiter";
		public const string MigrationWildcard = "migration.wildcard";
		public const string MigrationDownRequired = "migration.down-required";
		public const string MigrationForcePaths = "migration.force-paths";
		public const string MigrationPath = "migration.path";
		public const string LogLevel = "log.level";
	}

	public static readonly IDictionary<string, string> Dict = new Dictionary<string, string>
	{
		{ Key.ConnectionString, string.Empty },
		{ Key.DatabaseProvider, ProviderName.PostgreSql },
		{ Key.SchemaName, "dingo" },
		{ Key.MigrationDelimiter, @"^--\s*down$" },
		{ Key.MigrationWildcard, "*.sql" },
		{ Key.MigrationDownRequired, false.ToString() },
		{ Key.MigrationForcePaths, string.Empty },
		{ Key.LogLevel, LogLevel.Information.ToString() },
	};
}
