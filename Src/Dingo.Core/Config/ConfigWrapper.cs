using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Core.Config
{
	/// <inheritdoc />
	internal class ConfigWrapper : IConfigWrapper
	{
		private IConfiguration _projectConfiguration;
		private readonly IConfiguration _defaultConfiguration;
		private readonly IConfigLoader _configLoader;
		private readonly IConfigSaver _configSaver;

		public string CheckTableExistenceProcedurePath
		{
			get => _projectConfiguration.CheckTableExistenceProcedurePath ?? _defaultConfiguration.CheckTableExistenceProcedurePath;
			set => _projectConfiguration.CheckTableExistenceProcedurePath = value;
		}
		public string DingoMigrationsRootPath
		{
			get => _projectConfiguration.DingoMigrationsRootPath ?? _defaultConfiguration.DingoMigrationsRootPath;
			set => _projectConfiguration.DingoMigrationsRootPath = value;
		}
		public string MigrationsSearchPattern
		{
			get => _projectConfiguration.MigrationsSearchPattern ?? _defaultConfiguration.MigrationsSearchPattern;
			set => _projectConfiguration.MigrationsSearchPattern = value;
		}
		public string ConnectionString
		{
			get => _projectConfiguration.ConnectionString ?? _defaultConfiguration.ConnectionString;
			set => _projectConfiguration.ConnectionString = value;
		}
		public string ProviderName
		{
			get => _projectConfiguration.ProviderName ?? _defaultConfiguration.ProviderName;
			set => _projectConfiguration.ProviderName = value;
		}
		public string MigrationSchema
		{
			get => _projectConfiguration.MigrationSchema ?? _defaultConfiguration.MigrationSchema;
			set => _projectConfiguration.MigrationSchema = value;
		}
		public string MigrationTable
		{
			get => _projectConfiguration.MigrationTable ?? _defaultConfiguration.MigrationTable;
			set => _projectConfiguration.MigrationTable = value;
		}

		public string ActiveConfigFile { get; private set; }
		public bool ConfigFileExists => !string.IsNullOrWhiteSpace(ActiveConfigFile);

		public ConfigWrapper(IConfigLoader configLoader, IConfigSaver configSaver)
		{
			_configLoader = configLoader ?? throw new ArgumentNullException(nameof(configLoader));
			_configSaver = configSaver ?? throw new ArgumentNullException(nameof(configSaver));
			_defaultConfiguration = new DefaultConfiguration();
			_projectConfiguration = new ProjectConfiguration();
		}

		/// <inheritdoc />
		public Task LoadAsync(CancellationToken cancellationToken = default)
		{
			return LoadAsync(null, cancellationToken);
		}

		/// <inheritdoc />
		public async Task LoadAsync(string configPath, CancellationToken cancellationToken = default)
		{
			var loadConfigResult = string.IsNullOrWhiteSpace(configPath) 
				? await _configLoader.LoadProjectConfigAsync(cancellationToken)
				: await _configLoader.LoadProjectConfigAsync(configPath, cancellationToken);

			ActiveConfigFile = loadConfigResult.ConfigPath;
			_projectConfiguration = loadConfigResult.Configuration;
			_defaultConfiguration.ProviderName = _projectConfiguration.ProviderName ?? _defaultConfiguration.ProviderName;
		}

		/// <inheritdoc />
		public Task SaveAsync(CancellationToken cancellationToken = default)
		{
			return SaveAsync(null, cancellationToken);
		}

		/// <inheritdoc />
		public Task SaveAsync(string configPath, CancellationToken cancellationToken = default)
		{
			return string.IsNullOrWhiteSpace(configPath) 
				? _configSaver.SaveProjectConfigAsync(_projectConfiguration, cancellationToken)
				: _configSaver.SaveProjectConfigAsync(_projectConfiguration, configPath, cancellationToken);
		}
	}
}