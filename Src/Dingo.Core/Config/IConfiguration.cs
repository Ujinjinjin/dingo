namespace Dingo.Core.Config;

/// <summary> Configuration abstraction </summary>
public interface IConfiguration
{
	/// <summary> Path to the procedure script of checking table existence </summary>
	string TableExistsProcedurePath { get; set; }

	/// <summary> Root path to dingo migration files </summary>
	string DingoMigrationsDir { get; set; }

	/// <summary> Pattern to search migration files in specified directory </summary>
	string MigrationsSearchPattern { get; set; }

	/// <summary> Database connection string </summary>
	string ConnectionString { get; set; }

	/// <summary> Database provider name </summary>
	string ProviderName { get; set; }

	/// <summary> Database schema for you migrations </summary>
	string MigrationSchema { get; set; }

	/// <summary> Database table, where all migrations are stored </summary>
	string MigrationTable { get; set; }

	/// <summary> Configurable logging level </summary>
	int? LogLevel { get; set; }
}