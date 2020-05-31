using Dingo.Cli.Factories;
using Dingo.Cli.Operations;
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
            var configuration = new PostgresConfiguration();
            var databaseContextFactory = new DatabaseContextFactory();
            var operations = new DatabaseOperations(pathHelper, configuration, databaseContextFactory);
            var programOperations = new ProgramOperations(configuration, operations, scanner, hashMaker, pathHelper);

            await programOperations.RunAsync(args);
        }
    }
}
