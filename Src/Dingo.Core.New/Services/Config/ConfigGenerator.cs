using Dingo.Core.Extensions;
using Dingo.Core.Services.Adapters;
using Trico.Configuration;

namespace Dingo.Core.Services.Config;

internal class ConfigGenerator : IConfigGenerator
{
	private readonly IFileAdapter _fileAdapter;
	private readonly IPathAdapter _pathAdapter;
	private readonly IDirectoryAdapter _directoryAdapter;

	public ConfigGenerator(
		IFileAdapter fileAdapter,
		IPathAdapter pathAdapter,
		IDirectoryAdapter directoryAdapter
	)
	{
		_fileAdapter = fileAdapter.Required(nameof(fileAdapter));
		_pathAdapter = pathAdapter.Required(nameof(pathAdapter));
		_directoryAdapter = directoryAdapter.Required(nameof(directoryAdapter));
	}

	public void Generate(string? path = default, string? profile = default)
	{
		path ??= Constants.CurrentDir;

		var fullPath = _pathAdapter.Join(path, Constants.ConfigDir);
		if (!_directoryAdapter.Exists(fullPath))
		{
			_directoryAdapter.CreateDirectory(fullPath);
		}

		var configFilename = string.IsNullOrEmpty(profile)
			? $"{Constants.ConfigFilename}.{Constants.ConfigExtension}"
			: $"{Constants.ConfigFilename}.{profile}.{Constants.ConfigExtension}";

		var configPath = _pathAdapter.Join(fullPath, configFilename);
		if (!_fileAdapter.Exists(configPath))
		{
			_fileAdapter.Create(configPath);
		}
	}
}
