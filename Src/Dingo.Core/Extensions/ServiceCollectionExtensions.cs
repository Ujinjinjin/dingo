using Dingo.Core.Config;
using Dingo.Core.Facades;
using Dingo.Core.Factories;
using Dingo.Core.Helpers;
using Dingo.Core.Operations;
using Dingo.Core.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.Core.Extensions
{
	/// <summary> Collection of extensions for <see cref="IServiceCollection"/> </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary> Add dingo dependencies to service collection </summary>
		/// <param name="serviceCollection">Target service collection</param>
		public static void AddDingo(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IConfigLoader, ConfigLoader>();
			serviceCollection.AddSingleton<IConfigSaver, ConfigSaver>();
			serviceCollection.AddSingleton<IConfigWrapper, ConfigWrapper>();
			
			serviceCollection.AddSingleton<IDatabaseContextFactory, DatabaseContextFactory>();
			serviceCollection.AddSingleton<IInternalSerializerFactory, InternalSerializerFactory>();
			
			serviceCollection.AddSingleton<IDatabaseHelper, DatabaseHelper>();
			serviceCollection.AddSingleton<IDirectoryScanner, DirectoryScanner>();
			serviceCollection.AddSingleton<IHashMaker, HashMaker>();
			serviceCollection.AddSingleton<IPathHelper, PathHelper>();
			
			serviceCollection.AddSingleton<IFileFacade, FileFacade>();
			
			serviceCollection.AddSingleton<IMigrationOperations, MigrationOperations>();
			serviceCollection.AddSingleton<IConfigOperations, ConfigOperations>();
			serviceCollection.AddSingleton<IProviderOperations, ProviderOperations>();
			
			serviceCollection.AddSingleton<IDatabaseContext, DatabaseContext>();
		}
	}
}