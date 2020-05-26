using Dingo.Cli.Factories;
using Dingo.Cli.Operations;
using System;
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
            var operations = new PostgresOperations(pathHelper, configuration, databaseContextFactory);

            await operations.InstallDingoProceduresAsync();
            
            var result = await operations.CheckMigrationTableExistenceAsync();
            Console.WriteLine($"Migration table exists: {result}");

            // var filenameList = await scanner.GetFileListAsync(pathHelper.GetApplicationBasePath(), configuration.DingoDatabaseScriptsMask);
            //
            // foreach (var filename in filenameList)
            // {
            //     Console.WriteLine(filename);
            //     Console.WriteLine(await hashMaker.GetFileHashAsync(filename));
            // }
        }
    }
}
