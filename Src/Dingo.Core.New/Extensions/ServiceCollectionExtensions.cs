using Dingo.Core.IO;
using Dingo.Core.Repository;
using Dingo.Core.Repository.Command;
using Dingo.Core.Repository.Source;
using Dingo.Core.Services.Adapters;
using Dingo.Core.Services.Config;
using Dingo.Core.Services.Handlers;
using Dingo.Core.Services.Helpers;
using Dingo.Core.Services.Migrations;
using Dingo.Core.Validators.Migration;
using Dingo.Core.Validators.Migration.Name;
using Dingo.Core.Validators.Migration.Sql;
using Dingo.Core.Validators.Primitive;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Trico.Extensions;

namespace Dingo.Core.Extensions;

public static class ServiceCollectionExtensions
{
	public static void AddDingo(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddAdapters();
		serviceCollection.AddValidators();
		serviceCollection.AddFactories();
		serviceCollection.AddServices();

		serviceCollection.AddConfiguration()
			.AddEnvironmentVariableProvider()
			.AddFileProvider()
			.AddInMemoryProvider(Configuration.Dict);
	}

	private static void AddAdapters(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IDirectoryAdapter, DirectoryAdapter>();
		serviceCollection.AddSingleton<IFileAdapter, FileAdapter>();
		serviceCollection.AddSingleton<IPathAdapter, PathAdapter>();
	}

	private static void AddValidators(this IServiceCollection serviceCollection)
	{
		// migration validator
		serviceCollection.AddSingleton<MigrationValidator>();

		serviceCollection.AddSingleton<ISqlCommandValidator, UpSqlRequiredValidator>();
		serviceCollection.AddSingleton<ISqlCommandValidator, DownSqlRequiredValidator>();
		serviceCollection.AddSingleton<IMigrationNameValidator, MigrationNameValidator>();

		// primitive
		serviceCollection.AddSingleton<StringRequiredValidator>();
	}

	private static void AddFactories(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IConnectionFactory, ConnectionFactory>();
		serviceCollection.AddSingleton<ICommandProviderFactory, CommandProviderFactory>();
		serviceCollection.AddSingleton<ICommandProviderFactory, CommandProviderFactory>();
	}

	private static void AddServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IMigrationCommandParser, MigrationCommandParser>();
		serviceCollection.AddSingleton<IMigrationScanner, MigrationScanner>();
		serviceCollection.AddSingleton<IMigrationComparer, MigrationComparer>();
		serviceCollection.AddSingleton<IMigrationGenerator, MigrationGenerator>();
		serviceCollection.AddSingleton<IMigrationGenerator, MigrationGenerator>();
		serviceCollection.AddSingleton<IDirectoryScanner, DirectoryScanner>();
		serviceCollection.AddSingleton<IMigrationApplier, MigrationApplier>();
		serviceCollection.AddSingleton<IMigrationPathBuilder, MigrationPathBuilder>();
		serviceCollection.AddSingleton<IMigrationRunner, MigrationRunner>();
		serviceCollection.AddSingleton<IConfigGenerator, ConfigGenerator>();
		serviceCollection.AddSingleton<IRepository, DatabaseRepository>();
		serviceCollection.AddSingleton<INpgsqlDataSourceProvider, NpgsqlDataSourceProvider>();
		serviceCollection.AddSingleton<INpgsqlDataSourceBuilder, NpgsqlDataSourceBuilderAdapter>();
		serviceCollection.AddSingleton<IMigrationHandler, MigrationHandler>();
		serviceCollection.AddSingleton<IConnectionHandler, ConnectionHandler>();
		serviceCollection.AddSingleton<IConfigHandler, ConfigHandler>();

		serviceCollection.AddSingleton<ILoggerProvider, FileLoggerProvider>();
		serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
	}
}
