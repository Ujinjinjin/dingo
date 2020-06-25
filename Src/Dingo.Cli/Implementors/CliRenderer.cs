using Cliff.ConsoleUtils;
using Dingo.Core.Abstractions;
using Dingo.Core.Config;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace Dingo.Cli.Implementors
{
	/// <summary> CLI renderer </summary>
	[UsedImplicitly]
	public class CliRenderer : IRenderer
	{
		private readonly IConsoleQueue _consoleQueue;

		public CliRenderer(IConsoleQueue consoleQueue)
		{
			_consoleQueue = consoleQueue ?? throw new ArgumentNullException(nameof(consoleQueue));
		}

		/// <inheritdoc />
		public async Task ShowConfigAsync(IConfigWrapper configWrapper)
		{
			var configFileString = string.IsNullOrWhiteSpace(configWrapper.ActiveConfigFile)
				? "No compatible config file found. Showing default configs\n"
				: $"Using configs from: {configWrapper.ActiveConfigFile}\n";
			
			await _consoleQueue.EnqueueOutputAsync(configFileString);
			
			var configString = $"        connection string: {configWrapper.ConnectionString}\n" +
							   $"            provider name: {configWrapper.ProviderName}\n" +
							   $"         migration schema: {configWrapper.MigrationSchema}\n" +
							   $"          migration table: {configWrapper.MigrationTable}\n" +
							   $"migrations search pattern: {configWrapper.MigrationsSearchPattern}\n";

			await _consoleQueue.EnqueueOutputAsync(configString);
		}

		/// <inheritdoc />
		public async Task ShowMessage(string message)
		{
			await _consoleQueue.EnqueueOutputAsync(message);
		}
	}
}
