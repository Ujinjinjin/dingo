using Dingo.Core.Config;
using Dingo.Core.Repository;
using Dingo.Core.Repository.DbClasses;
using LinqToDB;
using Npgsql;
using System;

namespace Dingo.Core.Factories
{
	/// <inheritdoc />
	internal class DatabaseContextFactory : IDatabaseContextFactory
	{
		private readonly IConfigWrapper _configWrapper;
		
		public DatabaseContextFactory(IConfigWrapper configWrapper)
		{
			_configWrapper = configWrapper ?? throw new ArgumentNullException(nameof(configWrapper));
		}
		
		/// <inheritdoc />
		/// <exception cref="ArgumentOutOfRangeException"> Specified database provider not supported yet </exception>
		public IDatabaseContext CreateDatabaseContext()
		{
			switch (_configWrapper.ProviderName)
			{
				case ProviderName.PostgreSQL95:
					NpgsqlConnection.GlobalTypeMapper.MapComposite<DbMigrationInfoInput>("t_migration_info_input");
					return new DatabaseContext(_configWrapper.ProviderName, _configWrapper.ConnectionString);
				default:
					throw new ArgumentOutOfRangeException(_configWrapper.ProviderName);
			}
		}
	}
}