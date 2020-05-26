using Dingo.Cli.Operations;
using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var operations = new PostgresOperations(pathHelper, configuration);

            // var providers = DataConnection.GetRegisteredProviders();

            await operations.CheckSystemTableExistence();

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
