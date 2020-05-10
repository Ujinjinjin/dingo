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
            var path = "D:/gallk/Work/Utils/dingo/";

            var filenameList = await scanner.GetFileListAsync(path, "*.sql");

            foreach (var filename in filenameList)
            {
                Console.WriteLine(filename);
                Console.WriteLine(await hashMaker.GetFileHashAsync(filename));
            }
        }
    }
}
