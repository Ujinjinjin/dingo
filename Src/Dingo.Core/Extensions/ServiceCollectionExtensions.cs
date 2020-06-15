using Dingo.Core.Config;
using Dingo.Core.Factories;
using Dingo.Core.Operations;
using Dingo.Core.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.Core.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddDingoDependencies(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IConfigLoader, ConfigLoader>();
			serviceCollection.AddSingleton<IConfigSaver, ConfigSaver>();
			serviceCollection.AddSingleton<IConfigWrapper, ConfigWrapper>();
			
			serviceCollection.AddSingleton<IDatabaseContextFactory, DatabaseContextFactory>();
			serviceCollection.AddSingleton<IInternalSerializerFactory, InternalSerializerFactory>();
			
			serviceCollection.AddSingleton<IDatabaseOperations, DatabaseOperations>();
			serviceCollection.AddSingleton<IDirectoryScanner, DirectoryScanner>();
			serviceCollection.AddSingleton<IHashMaker, HashMaker>();
			serviceCollection.AddSingleton<IPathHelper, PathHelper>();
			serviceCollection.AddSingleton<IProgramOperations, ProgramOperations>();
			
			serviceCollection.AddSingleton<IDatabaseContext, DatabaseContext>();
		}
	}
}