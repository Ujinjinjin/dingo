using System;

namespace Dingo.Core.Config
{
	internal class DefaultConfiguration : IConfiguration
	{
		public string CheckTableExistenceProcedurePath
		{
			get => $"Database/{ProviderName}/procedures/system__check_table_existence.sql";
			set => throw new NotImplementedException();
		}

		public string DingoMigrationsRootPath
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

		public string ProviderName
		{
			get => LinqToDB.ProviderName.PostgreSQL95;
			set => throw new NotImplementedException();
		}

		public string MigrationSchema
		{
			get => "public";
			set => throw new NotImplementedException();
		}

		public string MigrationTable
		{
			get => "dingo_migration";
			set => throw new NotImplementedException();
		}
	}
}
