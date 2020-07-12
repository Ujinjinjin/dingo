using Cliff.ConsoleUtils;
using Dingo.Core.Abstractions;
using Dingo.Core.Config;
using Dingo.Core.Extensions;
using Dingo.Core.Models;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
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
		public async Task ShowMessageAsync(string message)
		{
			await _consoleQueue.EnqueueOutputAsync(message);
		}

		/// <inheritdoc />
		public async Task PrintTextAsync(string text, bool silent)
		{
			if (silent)
			{
				return;
			}
			await _consoleQueue.EnqueueOutputAsync(text);
		}

		/// <inheritdoc />
		public async Task PrintBreakLineAsync(
			bool silent,
			int? length = null,
			char symbol = '-',
			bool newLineBefore = true,
			bool newLineAfter = true
		)
		{
			if (silent)
			{
				return;
			}
			await _consoleQueue.EnqueueBreakLine(length, symbol, newLineBefore, newLineAfter);
		}

		/// <inheritdoc />
		public async Task ListItemsAsync(IList<string> itemList)
		{
			for (var i = 0; i < itemList.Count; i++)
			{
				await _consoleQueue.EnqueueOutputAsync($"{i + 1}. {itemList[i]}");
			}
		}

		/// <inheritdoc />
		public async Task ShowMigrationsStatusAsync(IList<MigrationInfo> migrationInfoList, bool silent)
		{
			await _consoleQueue.EnqueueBreakLine(newLineAfter: false);
			await _consoleQueue.EnqueueOutputAsync($"Total migrations count: {migrationInfoList.Count}.");

			if (silent)
			{
				var newCount = 0;
				var outdatedCount = 0;
				var upToDateCount = 0;

				for (var i = 0; i < migrationInfoList.Count; i++)
				{
					switch (migrationInfoList[i].Status)
					{
						case MigrationStatus.New:
							newCount++;
							break;
						case MigrationStatus.Outdated:
							outdatedCount++;
							break;
						case MigrationStatus.UpToDate:
							upToDateCount++;
							break;
						default:
							continue;
					}
				}

				await _consoleQueue.EnqueueOutputAsync($"New: {newCount}");
				await _consoleQueue.EnqueueOutputAsync($"Outdated: {outdatedCount}");
				await _consoleQueue.EnqueueOutputAsync($"Up to date: {upToDateCount}");
			}
			else
			{
				for (var i = 0; i < migrationInfoList.Count; i++)
				{
					var migrationInfo = migrationInfoList[i];

					await _consoleQueue.EnqueueOutputAsync($"{i + 1}. {migrationInfo.Path.Relative}");
					await _consoleQueue.EnqueueOutputAsync($"Hash: {migrationInfo.NewHash}");
					await _consoleQueue.EnqueueOutputAsync($"Status: {migrationInfo.Status.ToDisplayText()}\n");
				}
			}
			await _consoleQueue.EnqueueBreakLine(newLineBefore: false);
		}
	}
}
