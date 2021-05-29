using Dingo.Core.Abstractions;
using Dingo.Core.Config;
using Dingo.Core.Constants;
using Dingo.Core.Models;
using Dingo.Core.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	/// <inheritdoc />
	internal sealed class ProviderOperations : IProviderOperations
	{
		private readonly IRenderer _renderer;
		private readonly IPrompt _prompt;
		private readonly IConfigWrapper _configWrapper;
		private readonly ILogger _logger;

		public ProviderOperations(
			IRenderer renderer,
			IPrompt prompt,
			IConfigWrapper configWrapper, 
			ILoggerFactory loggerFactory
		)
		{
			_renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
			_prompt = prompt ?? throw new ArgumentNullException(nameof(prompt));
			_configWrapper = configWrapper ?? throw new ArgumentNullException(nameof(configWrapper));
			_logger = loggerFactory?.CreateLogger<ProviderOperations>() ?? throw new ArgumentNullException(nameof(loggerFactory));
		}

		/// <inheritdoc />
		public async Task ChooseDatabaseProviderAsync(string configPath = null)
		{
			using var _ = new CodeTiming(_logger);

			await _configWrapper.LoadAsync(configPath);

			_configWrapper.ProviderName = _prompt.Choose(
				"Please, choose database provider suitable to your project",
				DbProvider.SupportedDatabaseProviderNames
			);

			await _configWrapper.SaveAsync(configPath);
			await _renderer.ShowMessageAsync($"Database provider successfully updated to `{_configWrapper.ProviderName}`", MessageType.Info);
		}

		/// <inheritdoc />
		public async Task ListSupportedDatabaseProvidersAsync()
		{
			using var _ = new CodeTiming(_logger);

			await _renderer.ListItemsAsync(DbProvider.SupportedDatabaseProviderNames);
		}

		/// <inheritdoc />
		public async Task ValidateDatabaseProviderAsync(string configPath = null)
		{
			using var _ = new CodeTiming(_logger);

			await _configWrapper.LoadAsync(configPath);

			if (DbProvider.SupportedDatabaseProviderNames.Contains(_configWrapper.ProviderName))
			{
				await _renderer.ShowMessageAsync($"Chosen database provider `{_configWrapper.ProviderName}` is supported", MessageType.Info);
			}
			else
			{
				await _renderer.ShowMessageAsync($"Chosen database provider `{_configWrapper.ProviderName}` is not supported yet", MessageType.Warning);
			}
		}
	}
}
