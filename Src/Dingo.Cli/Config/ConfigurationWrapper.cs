﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Cli.Config
{
	public class ConfigurationWrapper : IConfigurationWrapper
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

		public ConfigurationWrapper(IConfigLoader configLoader, IConfigSaver configSaver)
		{
			_configLoader = configLoader ?? throw new ArgumentNullException(nameof(configLoader));
			_configSaver = configSaver ?? throw new ArgumentNullException(nameof(configSaver));
			_defaultConfiguration = new DefaultConfiguration();
		}

		public Task LoadAsync(CancellationToken cancellationToken = default)
		{
			return LoadAsync(null, cancellationToken);
		}

		public async Task LoadAsync(string configPath, CancellationToken cancellationToken = default)
		{
			_projectConfiguration = string.IsNullOrWhiteSpace(configPath) 
				? await _configLoader.LoadProjectConfigAsync(cancellationToken)
				: await _configLoader.LoadProjectConfigAsync(configPath, cancellationToken);
		}

		public Task SaveAsync(CancellationToken cancellationToken = default)
		{
			return SaveAsync(null, cancellationToken);
		}

		public Task SaveAsync(string configPath, CancellationToken cancellationToken = default)
		{
			return string.IsNullOrWhiteSpace(configPath) 
				? _configSaver.SaveProjectConfigAsync(_projectConfiguration, cancellationToken)
				: _configSaver.SaveProjectConfigAsync(_projectConfiguration, configPath, cancellationToken);
		}
	}
}