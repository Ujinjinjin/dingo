using Dingo.Core.Abstractions;
using Dingo.Core.Config;
using Dingo.Core.Helpers;
using Dingo.Core.Models;
using Dingo.Core.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	/// <inheritdoc />
	internal class LogsOperations : ILogsOperations
	{
		private readonly IConfigWrapper _configWrapper;
		private readonly IPathHelper _pathHelper;
		private readonly IPrompt _prompt;
		private readonly IRenderer _renderer;
		private readonly ILogger _logger;

		public LogsOperations(
			IPathHelper pathHelper,
			IConfigWrapper configWrapper,
			IPrompt prompt,
			IRenderer renderer,
			ILoggerFactory loggerFactory
		)
		{
			_configWrapper = configWrapper ?? throw new ArgumentNullException(nameof(configWrapper));
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
			_prompt = prompt ?? throw new ArgumentNullException(nameof(prompt));
			_renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
			_logger = loggerFactory?.CreateLogger<LogsOperations>() ?? throw new ArgumentNullException(nameof(loggerFactory));
		}

		/// <inheritdoc />
		public async Task SwitchLogLevelAsync(string configPath = null, int? logLevel = null)
		{
			using var _ = new CodeTiming(_logger);

			await _configWrapper.LoadAsync(configPath);

			LogLevel logLevelEnum;
			if (logLevel.HasValue)
			{
				logLevelEnum = (LogLevel) logLevel;
			}
			else
			{
				logLevelEnum = _prompt.Choose(
					"Please, choose desired level of logging",
					new[] { LogLevel.Trace, LogLevel.Debug, LogLevel.Information, LogLevel.Warning, LogLevel.Error, LogLevel.Critical, LogLevel.None, },
					x => $"{x.ToString()} ({(int) x})"
				);
			}

			_configWrapper.LogLevel = (int) logLevelEnum;

			await _configWrapper.SaveAsync(configPath);
			await _renderer.ShowMessageAsync($"Logging level set to `{logLevelEnum.ToString()}`", MessageType.Info);
		}

		/// <inheritdoc />
		public Task PruneLogsAsync()
		{
			using var _ = new CodeTiming(_logger);

			var directoryInfo = new DirectoryInfo(_pathHelper.GetLogsDirectory());

			foreach (var file in directoryInfo.EnumerateFiles())
			{
				file.Delete();
			}

			foreach (var dir in directoryInfo.EnumerateDirectories())
			{
				dir.Delete(true);
			}

			return Task.CompletedTask;
		}
	}
}
