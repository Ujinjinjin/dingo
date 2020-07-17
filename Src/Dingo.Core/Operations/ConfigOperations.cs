using Dingo.Core.Abstractions;
using Dingo.Core.Config;
using Dingo.Core.Models;
using System;
using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	/// <inheritdoc />
	internal class ConfigOperations : IConfigOperations
	{
		private readonly IConfigWrapper _configWrapper;
		private readonly IPrompt _prompt;
		private readonly IRenderer _renderer;

		public ConfigOperations(
			IConfigWrapper configWrapper,
			IPrompt prompt,
			IRenderer renderer
		)
		{
			_configWrapper = configWrapper ?? throw new ArgumentNullException(nameof(configWrapper));
			_prompt = prompt ?? throw new ArgumentNullException(nameof(prompt));
			_renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
		}

		/// <inheritdoc />
		public async Task InitConfigurationFileAsync(string configPath = null)
		{
			await _configWrapper.LoadAsync(configPath);

			if (!_configWrapper.ConfigFileExists || _prompt.Confirm($"Config file {_configWrapper.ActiveConfigFile} already exists, do you want to override it?"))
			{
				_configWrapper.ConnectionString = string.Empty;
				_configWrapper.ProviderName = string.Empty;
				await _configWrapper.SaveAsync(configPath);

				await _renderer.ShowMessageAsync("Dingo config file successfully initialized!", MessageType.Info);	
			}
		}

		/// <inheritdoc />
		public async Task ShowProjectConfigurationAsync(string configPath = null)
		{
			await _configWrapper.LoadAsync(configPath);

			await _renderer.ShowConfigAsync(_configWrapper);
		}

		/// <inheritdoc />
		public async Task UpdateProjectConfigurationAsync(
			string configPath = null,
			string connectionString = null,
			string providerName = null,
			string migrationSchema = null,
			string migrationTable = null,
			string searchPattern = null
		)
		{
			await _configWrapper.LoadAsync(configPath);

			_configWrapper.ConnectionString = string.IsNullOrWhiteSpace(connectionString)
				? _configWrapper.ConnectionString
				: connectionString;

			_configWrapper.ProviderName = string.IsNullOrWhiteSpace(providerName)
				? _configWrapper.ProviderName
				: providerName;

			_configWrapper.MigrationSchema = string.IsNullOrWhiteSpace(migrationSchema)
				? _configWrapper.MigrationSchema
				: migrationSchema;

			_configWrapper.MigrationTable = string.IsNullOrWhiteSpace(migrationTable)
				? _configWrapper.MigrationTable
				: migrationTable;

			_configWrapper.MigrationsSearchPattern = string.IsNullOrWhiteSpace(searchPattern)
				? _configWrapper.MigrationsSearchPattern
				: searchPattern;

			await _configWrapper.SaveAsync(configPath);
		}
	}
}
