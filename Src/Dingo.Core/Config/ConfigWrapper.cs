namespace Dingo.Core.Config;

/// <inheritdoc />
internal sealed class ConfigWrapper : IConfigWrapper
{
	private IConfiguration _projectConfiguration;
	private readonly IConfiguration _defaultConfiguration;
	private readonly IConfigReader _configReader;
	private readonly IConfigWriter _configWriter;

	public string TableExistsProcedurePath
	{
		get => _projectConfiguration.TableExistsProcedurePath ?? _defaultConfiguration.TableExistsProcedurePath;
		set => _projectConfiguration.TableExistsProcedurePath = value;
	}
	public string DingoMigrationsDir
	{
		get => _projectConfiguration.DingoMigrationsDir ?? _defaultConfiguration.DingoMigrationsDir;
		set => _projectConfiguration.DingoMigrationsDir = value;
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

	public int? LogLevel
	{
		get => _projectConfiguration.LogLevel ?? _defaultConfiguration.LogLevel ?? (int)Microsoft.Extensions.Logging.LogLevel.None;
		set => _projectConfiguration.LogLevel = value;
	}

	public string ActiveConfigFile { get; private set; }
	public bool ConfigFileExists => !string.IsNullOrWhiteSpace(ActiveConfigFile);

	public ConfigWrapper(IConfigReader configReader, IConfigWriter configWriter)
	{
		_configReader = configReader ?? throw new ArgumentNullException(nameof(configReader));
		_configWriter = configWriter ?? throw new ArgumentNullException(nameof(configWriter));
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
			? await _configReader.LoadProjectConfigAsync(cancellationToken)
			: await _configReader.LoadProjectConfigAsync(configPath, cancellationToken);

		ActiveConfigFile = loadConfigResult.ConfigPath;
		_projectConfiguration = loadConfigResult.Configuration;

		_defaultConfiguration.ProviderName = _projectConfiguration.ProviderName ?? _defaultConfiguration.ProviderName;
		_defaultConfiguration.LogLevel = _projectConfiguration.LogLevel ?? _defaultConfiguration.LogLevel;
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
			? _configWriter.SaveProjectConfigAsync(_projectConfiguration, cancellationToken)
			: _configWriter.SaveProjectConfigAsync(_projectConfiguration, configPath, cancellationToken);
	}
}