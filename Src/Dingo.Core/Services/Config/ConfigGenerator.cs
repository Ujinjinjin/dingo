using Dingo.Core.Extensions;
using Dingo.Core.Services.Adapters;
using Dingo.Core.Utils;

namespace Dingo.Core.Services.Config;

internal sealed class ConfigGenerator : IConfigGenerator
{
	private readonly IFile _file;
	private readonly IPath _path;
	private readonly IDirectory _directory;

	public ConfigGenerator(
		IFile file,
		IPath path,
		IDirectory directory
	)
	{
		_file = file.Required(nameof(file));
		_path = path.Required(nameof(path));
		_directory = directory.Required(nameof(directory));
	}

	public void Generate(string? path = default, string? profile = default)
	{
		path ??= Constants.CurrentDir;

		var fullPath = _path.Join(path, Constants.ConfigDir);
		if (!_directory.Exists(fullPath))
		{
			_directory.CreateDirectory(fullPath);
		}

		var configFilename = ConfigFilename.Build(profile);
		var configPath = _path.Join(fullPath, configFilename);
		if (!_file.Exists(configPath))
		{
			_file.Create(configPath);
		}
	}
}
