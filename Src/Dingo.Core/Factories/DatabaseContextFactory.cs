using Dingo.Core.Config;
using Dingo.Core.Repository;
using LinqToDB;
using Microsoft.Extensions.Logging;
using System;

namespace Dingo.Core.Factories
{
	/// <inheritdoc />
	internal sealed class DatabaseContextFactory : IDatabaseContextFactory
	{
		private readonly IConfigWrapper _configWrapper;
		private readonly IDatabaseContractConverterFactory _databaseContractConverterFactory;
		private readonly ILoggerFactory _loggerFactory;
		private readonly ILogger _logger;

		public DatabaseContextFactory(
			IConfigWrapper configWrapper,
			IDatabaseContractConverterFactory databaseContractConverterFactory,
			ILoggerFactory loggerFactory
		)
		{
			_configWrapper = configWrapper ?? throw new ArgumentNullException(nameof(configWrapper));
			_databaseContractConverterFactory = databaseContractConverterFactory ?? throw new ArgumentNullException(nameof(databaseContractConverterFactory));
			_loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
			_logger = loggerFactory?.CreateLogger<DatabaseContextFactory>() ?? throw new ArgumentNullException(nameof(loggerFactory));
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentOutOfRangeException">Specified database provider not supported yet</exception>
		public IDatabaseContext CreateDatabaseContext()
		{
			return _configWrapper.ProviderName switch
			{
				ProviderName.PostgreSQL95 => new DatabaseContext(
					_configWrapper.ProviderName,
					_configWrapper.ConnectionString,
					_loggerFactory,
					_databaseContractConverterFactory.CreatePostgresContractConverter()
				),
				ProviderName.SqlServer2017 => new DatabaseContext(
					_configWrapper.ProviderName,
					_configWrapper.ConnectionString,
					_loggerFactory,
					_databaseContractConverterFactory.CreateSqlServerContractConverter()
				),
				_ => throw new ArgumentOutOfRangeException(_configWrapper.ProviderName)
			};
		}
	}
}