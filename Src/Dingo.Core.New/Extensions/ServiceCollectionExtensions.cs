using Dingo.Core.New.Factories;
using Dingo.Core.New.Validators;
using Dingo.Core.New.Validators.MigrationValidator;
using Dingo.Core.New.Validators.Primitive;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.Core.New.Extensions;

public static class ServiceCollectionExtensions
{
	public static void UseDingo(this IServiceCollection serviceCollection)
	{
		serviceCollection.UseValidators();
		serviceCollection.UseFactories();
	}

	private static void UseValidators(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IMigrationValidator, MigrationValidator>();
		serviceCollection.AddSingleton<MigrationUpSqlRequiredValidator>();
		serviceCollection.AddSingleton<IValidator<Migration>>(x => x.GetRequiredService<MigrationUpSqlRequiredValidator>());
		serviceCollection.AddSingleton<MigrationDownSqlRequiredValidator>();
		serviceCollection.AddSingleton<IValidator<Migration>>(x => x.GetRequiredService<MigrationDownSqlRequiredValidator>());

		// primitive
		serviceCollection.AddSingleton<StringRequiredValidator>();
	}

	private static void UseFactories(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IMigrationFactory, MigrationFactory>();
	}
}
