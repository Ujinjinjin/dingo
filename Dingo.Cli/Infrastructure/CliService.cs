using Dingo.Abstractions;
using Dingo.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Dingo.Cli.Infrastructure
{
	internal class CliService : ICliService
	{
		public IServiceProvider ServiceProvider { get; }
		
		public CliService(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}
		
		public async Task ExecuteAsync(string[] args)
		{
			var root = ServiceProvider.GetService<RootCommand>();

			this.MapControllers(root);

			await root.InvokeAsync(args);
		}
	}
}
