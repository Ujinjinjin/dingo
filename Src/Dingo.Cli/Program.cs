using Dingo.Core.Config;
using Dingo.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Dingo.Cli
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var collection = new ServiceCollection();
            collection.AddDingoDependencies();
            var provider = collection.BuildServiceProvider();
            
            
            var configurationWrapper = provider.GetService<IConfigWrapper>();

            await configurationWrapper.LoadAsync();
            
            await configurationWrapper.SaveAsync();
        }
    }
}
