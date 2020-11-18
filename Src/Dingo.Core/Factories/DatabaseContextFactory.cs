using Dingo.Core.Config;
using Dingo.Core.Repository;
using Dingo.Core.Repository.DbClasses;
using LinqToDB;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;

namespace Dingo.Core.Factories
{
	/// <inheritdoc />
	internal class DatabaseContextFactory : IDatabaseContextFactory
	{
		private readonly IConfigWrapper _configWrapper;
		private readonly ILoggerFactory _loggerFactory;
		
		public DatabaseContextFactory(IConfigWrapper configWrapper, ILoggerFactory loggerFactory)
		{
			_configWrapper = configWrapper ?? throw new ArgumentNullException(nameof(configWrapper));
			_loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
		}
		
		/// <inheritdoc />
		/// <exception cref="ArgumentOutOfRangeException">Specified database provider not supported yet</exception>
		public IDatabaseContext CreateDatabaseContext()
		{
			switch (_configWrapper.ProviderName)
			{
				case ProviderName.PostgreSQL95:
					NpgsqlConnection.GlobalTypeMapper.MapComposite<DbMigrationInfoInput>("t_migration_info_input");
					return new DatabaseContext(_configWrapper.ProviderName, _configWrapper.ConnectionString, _loggerFactory);
				default:
					throw new ArgumentOutOfRangeException(_configWrapper.ProviderName);
			}
		}
	}
}