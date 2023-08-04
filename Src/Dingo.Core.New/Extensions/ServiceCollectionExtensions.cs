using Dingo.Core.Migrations;
using Dingo.Core.Validators;
using Dingo.Core.Validators.MigrationValidators;
using Dingo.Core.Validators.MigrationValidators.SqlValidators;
using Dingo.Core.Validators.Primitive;
using Microsoft.Extensions.DependencyInjection;
using Trico.Extensions;

namespace Dingo.Core.Extensions;

public static class ServiceCollectionExtensions
{
	public static void AddDingo(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddValidators();
		serviceCollection.AddFactories();

		serviceCollection.AddConfiguration()
			.AddEnvironmentVariableProvider()
			.AddFileProvider()
			.AddInMemoryProvider(Configuration.Dict);
	}

	private static void AddValidators(this IServiceCollection serviceCollection)
	{
		// migration validator
		serviceCollection.AddSingleton<MigrationValidator>();
		serviceCollection.AddSingleton<IMigrationValidator>(
			x => x.GetRequiredService<MigrationValidator>()
		);

		serviceCollection.AddSingleton<MigrationUpSqlRequiredValidator>();
		serviceCollection.AddSingleton<IValidatorGroupMember<Migration, MigrationValidator>>(
			x => x.GetRequiredService<MigrationUpSqlRequiredValidator>()
		);

		serviceCollection.AddSingleton<MigrationDownSqlRequiredValidator>();
		serviceCollection.AddSingleton<IValidatorGroupMember<Migration, MigrationValidator>>(
			x => x.GetRequiredService<MigrationDownSqlRequiredValidator>()
		);

		// primitive
		serviceCollection.AddSingleton<StringRequiredValidator>();
	}

	private static void AddFactories(this IServiceCollection serviceCollection)
	{
	}
}
