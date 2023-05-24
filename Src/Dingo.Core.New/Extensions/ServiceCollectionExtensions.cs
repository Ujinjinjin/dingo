using Dingo.Core.Validators;
using Dingo.Core.Validators.MigrationValidators;
using Dingo.Core.Validators.MigrationValidators.SqlValidators;
using Dingo.Core.Validators.Primitive;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.Core.Extensions;

public static class ServiceCollectionExtensions
{
	public static void UseDingo(this IServiceCollection serviceCollection)
	{
		serviceCollection.UseValidators();
		serviceCollection.UseFactories();
	}

	private static void UseValidators(this IServiceCollection serviceCollection)
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

	private static void UseFactories(this IServiceCollection serviceCollection)
	{
	}
}
