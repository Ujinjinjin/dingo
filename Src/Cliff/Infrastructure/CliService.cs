using Cliff.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace Cliff.Infrastructure
{
	/// <inheritdoc />
	public class CliService : ICliService
	{
		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; }
		
		public CliService(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}
		
		/// <inheritdoc />
		public async Task ExecuteAsync(string[] args)
		{
			await ServiceProvider.RegisterControllers()
				.GetService<RootCommand>()
				.InvokeAsync(args);
		}
	}
}