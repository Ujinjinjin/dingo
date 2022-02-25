using Dingo.Core.Adapters;
using Dingo.Core.Constants;
using Dingo.Core.Factories;
using Dingo.Core.Helpers;
using Dingo.Core.Models;

namespace Dingo.Core.Config;

/// <inheritdoc />
internal sealed class ConfigReader : IConfigReader
{
	private readonly IPathHelper _pathHelper;
	private readonly IFileAdapter _fileAdapter;
	private readonly IInternalSerializerFactory _internalSerializerFactory;

	public ConfigReader(
		IPathHelper pathHelper,
		IFileAdapter fileAdapter,
		IInternalSerializerFactory internalSerializerFactory
	)
	{
		_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
		_fileAdapter = fileAdapter ?? throw new ArgumentNullException(nameof(fileAdapter));
		_internalSerializerFactory = internalSerializerFactory ?? throw new ArgumentNullException(nameof(internalSerializerFactory));
	}

	/// <inheritdoc />
	public Task<LoadConfigResult> LoadProjectConfigAsync(CancellationToken cancellationToken = default)
	{
		return LoadProjectConfigAsync(
			_pathHelper.BuildFilePath(
				_pathHelper.GetExecutionBaseDirectory(),
				Defaults.DingoConfigFilename,
				Defaults.DingoConfigExtension
			),
			cancellationToken
		);
	}

	/// <inheritdoc />
	public async Task<LoadConfigResult> LoadProjectConfigAsync(string configPath, CancellationToken cancellationToken = default)
	{
		if (!Path.IsPathRooted(configPath))
		{
			configPath = _pathHelper.GetAbsolutePathFromRelative(configPath);
		}

		var internalSerializer = _internalSerializerFactory.CreateInternalSerializer(configPath);

		if (!_fileAdapter.Exists(configPath))
		{
			return new LoadConfigResult
			{
				Configuration = new ProjectConfiguration(),
				ConfigPath = null
			};
		}

		var fileContents = await _fileAdapter.ReadAllTextAsync(configPath, cancellationToken);

		return new LoadConfigResult
		{
			Configuration = internalSerializer.Deserialize<ProjectConfiguration>(fileContents),
			ConfigPath = configPath
		};
	}
}