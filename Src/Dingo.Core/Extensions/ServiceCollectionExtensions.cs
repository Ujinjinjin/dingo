using Dingo.Core.Adapters;
using Dingo.Core.Config;
using Dingo.Core.Factories;
using Dingo.Core.Helpers;
using Dingo.Core.Operations;
using Dingo.Core.Repository;
using Dingo.Core.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dingo.Core.Extensions
{
	/// <summary> Collection of extensions for <see cref="IServiceCollection"/> </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary> Add dingo dependencies to service collection </summary>
		/// <param name="serviceCollection">Target service collection</param>
		public static void UseDingo(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IConfigLoader, ConfigLoader>();
			serviceCollection.AddSingleton<IConfigSaver, ConfigSaver>();
			serviceCollection.AddSingleton<IConfigWrapper, ConfigWrapper>();

			serviceCollection.AddSingleton<IDatabaseContextFactory, DatabaseContextFactory>();
			serviceCollection.AddSingleton<IDatabaseContractConverterFactory, DatabaseContractConverterFactory>();
			serviceCollection.AddSingleton<IInternalSerializerFactory, InternalSerializerFactory>();
			serviceCollection.AddSingleton<ILoggerFactory, DingoLoggerFactory>();
			serviceCollection.AddSingleton<IOutputQueueFactory, OutputQueueFactory>();

			serviceCollection.AddSingleton<IDirectoryScanner, DirectoryScanner>();
			serviceCollection.AddSingleton<IHashMaker, HashMaker>();
			serviceCollection.AddSingleton<IPathHelper, PathHelper>();

			serviceCollection.AddSingleton<IDirectoryAdapter, DirectoryAdapter>();
			serviceCollection.AddSingleton<IFileAdapter, FileAdapter>();

			serviceCollection.AddSingleton<IConfigOperations, ConfigOperations>();
			serviceCollection.AddSingleton<ILogsOperations, LogsOperations>();
			serviceCollection.AddSingleton<IMigrationOperations, MigrationOperations>();
			serviceCollection.AddSingleton<IProviderOperations, ProviderOperations>();

			serviceCollection.AddSingleton<MigrationNameValidator>();

			serviceCollection.AddSingleton<IDatabaseRepository, DatabaseRepository>();
			serviceCollection.AddSingleton<IDatabaseContext, DatabaseContext>();
		}
	}
}