using Dingo.Abstractions.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Dingo.Cli.Infrastructure
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var serviceProvider = new IocModule()
                .Build();

            var dingo = serviceProvider.GetService<ICliService>();

            await dingo.ExecuteAsync(args);
        }
    }
}
