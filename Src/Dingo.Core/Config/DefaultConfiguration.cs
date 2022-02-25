using System;

namespace Dingo.Core.Config;

/// <summary> Default project configurations </summary>
internal sealed class DefaultConfiguration : IConfiguration
{
	public string TableExistsProcedurePath
	{
		get => $"Database/{ProviderName}/3. routines/dingo__table_exists.sql";
		set => throw new NotImplementedException();
	}

	public string DingoMigrationsDir
	{
		get => $"Database/{ProviderName}/";
		set => throw new NotImplementedException();
	}

	public string MigrationsSearchPattern
	{
		get => "*.sql";
		set => throw new NotImplementedException();
	}

	public string ConnectionString
	{
		get => "Server=172.18.223.189;Port=5432;Database=dingo_db;User Id=local_user;Password=qwer1234;";
		set => throw new NotImplementedException();
	}

	public string ProviderName { get; set; } = LinqToDB.ProviderName.PostgreSQL95;

	public string MigrationSchema
	{
		get
		{
			return ProviderName switch
			{
				LinqToDB.ProviderName.PostgreSQL => "public",
				LinqToDB.ProviderName.SqlServer2017 => "dbo",
				_ => null
			};
		}
		set => throw new NotImplementedException();
	}

	public string MigrationTable
	{
		get => "dingo_migration";
		set => throw new NotImplementedException();
	}

	public int? LogLevel { get; set; } = (int)Microsoft.Extensions.Logging.LogLevel.Information;
}