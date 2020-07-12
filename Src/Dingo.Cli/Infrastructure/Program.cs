using Cliff.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Dingo.Cli.Infrastructure
{
    /// <summary> Program`s entry point class </summary>
    internal static class Program
    {
        /// <summary> Program`s entry point method </summary>
        /// <param name="args">Arguments passed to application</param>
        private static async Task Main(string[] args)
        {
            var serviceProvider = new IocModule()
                .Build();

            var dingo = serviceProvider.GetService<ICliService>();

            await dingo.ExecuteAsync(args);
        }
    }
}
