using Dingo.Core.Adapters;
using Dingo.Core.Helpers;
using Dingo.Core.Migrations;
using Dingo.Core.Models;
using Dingo.Core.Validators;
using Dingo.Core.Validators.Migration;
using Dingo.Core.Validators.Migration.Sql;
using Dingo.Core.Validators.Primitive;
using Microsoft.Extensions.DependencyInjection;
using Trico.Extensions;

namespace Dingo.Core.Extensions;

public static class ServiceCollectionExtensions
{
	public static void AddDingo(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddAdapters();
		serviceCollection.AddMigrations();
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

	private static void AddMigrations(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IMigrationCommandParser, MigrationCommandParser>();
		serviceCollection.AddSingleton<IMigrationScanner, MigrationScanner>();
	}

	private static void AddValidators(this IServiceCollection serviceCollection)
	{
		// migration validator
		serviceCollection.AddSingleton<MigrationValidator>();

		serviceCollection.AddSingleton<ISqlCommandValidator, UpSqlRequiredValidator>();
		serviceCollection.AddSingleton<ISqlCommandValidator, DownSqlRequiredValidator>();

		// primitive
		serviceCollection.AddSingleton<StringRequiredValidator>();
	}

	private static void AddFactories(this IServiceCollection serviceCollection)
	{
	}

	private static void AddServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IDirectoryScanner, DirectoryScanner>();
	}
}
