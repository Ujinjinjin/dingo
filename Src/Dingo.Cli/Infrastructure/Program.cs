using Cliff.Infrastructure;
using Dingo.Core.IO;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
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
			var outputQueues = serviceProvider
				.GetServices<IOutputQueue>()
				.ToArray();

			await dingo.ExecuteAsync(args);

			while (!outputQueues.All(x => x.IsEmpty))
			{
				Thread.Sleep(100);
				// wait until queues are empty
			}
			Thread.Sleep(100);
		}
	}
}
