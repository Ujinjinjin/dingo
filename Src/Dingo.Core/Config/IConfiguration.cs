namespace Dingo.Core.Config
{
	/// <summary> Configuration abstraction </summary>
	public interface IConfiguration
	{
		/// <summary> Path to the procedure script of checking table existence </summary>
		string CheckTableExistenceProcedurePath { get; set; }

		/// <summary> Root path to dingo migration files </summary>
		string DingoMigrationsRootPath { get; set; }

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
	}
}
