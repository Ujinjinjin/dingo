using Cliff.ConsoleUtils;
using Dingo.Core.Abstractions;
using Dingo.Core.Config;
using Dingo.Core.Extensions;
using Dingo.Core.Models;
using JetBrains.Annotations;
using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
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
		public async Task ListItemsAsync(IList<string> itemList, TextStyle textStyle = TextStyle.Plain)
		{
			var stringBuilder = new StringBuilder();
			for (var i = 0; i < itemList.Count; i++)
			{
				stringBuilder.Append($"{i + 1}. {itemList[i]}");

				if (i != itemList.Count - 1)
				{
					stringBuilder.Append('\n');
				}
			}

			await PrintTextAsync(stringBuilder.ToString(), textStyle: textStyle);
		}

		/// <inheritdoc />
		public async Task PrintBreakLineAsync(
			bool silent = false,
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
		public async Task PrintTextAsync(string text, bool silent = false, TextStyle textStyle = TextStyle.Plain)
		{
			if (silent)
			{
				return;
			}

			var color = textStyle.ToColor();
			if (color != Color.Empty)
			{
				text = text.Pastel(color);
			}
			
			await _consoleQueue.EnqueueOutputAsync(text);
		}

		/// <inheritdoc />
		public async Task ShowConfigAsync(IConfigWrapper configWrapper)
		{
			var configFileString = string.IsNullOrWhiteSpace(configWrapper.ActiveConfigFile)
				? "No compatible config file found. Showing default configs\n"
				: $"Using configs from: {configWrapper.ActiveConfigFile}\n";

			await PrintTextAsync(configFileString, textStyle: TextStyle.Info);

			var configString = $"connection string: {configWrapper.ConnectionString}\n" +
							   $"    provider name: {configWrapper.ProviderName}\n" +
							   $" migration schema: {configWrapper.MigrationSchema}\n" +
							   $"  migration table: {configWrapper.MigrationTable}\n" +
							   $"   search pattern: {configWrapper.MigrationsSearchPattern}\n";

			await PrintTextAsync(configString, textStyle: TextStyle.Plain);
		}

		/// <inheritdoc />
		public async Task ShowMessageAsync(string message, MessageType messageType)
		{
			message = messageType.AddPrefixToMessage(message);
			var textStyle = messageType.ToTextStyle();

			await PrintTextAsync(message, textStyle: textStyle);
		}

		/// <inheritdoc />
		public async Task ShowMigrationsStatusAsync(IList<MigrationInfo> migrationInfoList, bool silent)
		{
			await PrintBreakLineAsync(newLineBefore: false, newLineAfter: false);
			await PrintTextAsync($"Total migrations count: {migrationInfoList.Count}.\n", textStyle: TextStyle.Info);

			var notUpToDateCount = 0;
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

				notUpToDateCount = newCount + outdatedCount;

				await PrintTextAsync($"New: {newCount}", textStyle: TextStyle.Plain);
				await PrintTextAsync($"Outdated: {outdatedCount}", textStyle: TextStyle.Plain);
				await PrintTextAsync($"Up to date: {upToDateCount}", textStyle: TextStyle.Plain);
			}
			else
			{
				for (var i = 0; i < migrationInfoList.Count; i++)
				{
					if (migrationInfoList[i].Status != MigrationStatus.UpToDate)
					{
						notUpToDateCount++;
					}

					await PrintTextAsync($"{i + 1}) '{migrationInfoList[i].Path.Relative}'", textStyle: TextStyle.Plain);
					await PrintTextAsync($"Hash: {migrationInfoList[i].NewHash[..10]}", textStyle: TextStyle.Plain);
					await PrintTextAsync($"Status: {migrationInfoList[i].Status.ToDisplayText()}\n", textStyle: TextStyle.Plain);
				}
			}

			await PrintBreakLineAsync(newLineBefore: false, newLineAfter: false);

			if (notUpToDateCount == 0)
			{
				await PrintTextAsync("Everything is up to date, no actions required.", textStyle: TextStyle.Success);	
			}
		}
	}
}
