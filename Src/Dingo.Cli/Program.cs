using Dingo.Cli.Config;
using Dingo.Cli.Factories;
using Dingo.Cli.Operations;
using Dingo.Cli.Serializers;
using System.Threading.Tasks;

namespace Dingo.Cli
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var scanner = new DirectoryScanner();
            var hashMaker = new HashMaker();
            var pathHelper = new PathHelper();
            var configuration = new DefaultConfiguration();
            var databaseContextFactory = new DatabaseContextFactory();
            var operations = new DatabaseOperations(pathHelper, configuration, databaseContextFactory);
            var programOperations = new ProgramOperations(configuration, operations, scanner, hashMaker, pathHelper);
            // var serializer = new JsonInternalSerializer();
            var serializer = new YamlInternalSerializer();

            // await programOperations.RunMigrationsAsync(args);
            
            var configReader = new ConfigLoader(pathHelper, serializer);
            var configSaver = new ConfigSaver(pathHelper, serializer);

            var config = await configReader.LoadProjectConfigAsync();
            
            await configSaver.SaveProjectConfigAsync(config);
        }
    }
}
