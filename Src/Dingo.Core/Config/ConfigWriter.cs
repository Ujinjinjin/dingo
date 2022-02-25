using Dingo.Core.Constants;
using Dingo.Core.Factories;
using Dingo.Core.Helpers;
using System.Text;

namespace Dingo.Core.Config;

/// <inheritdoc />
internal sealed class ConfigWriter : IConfigWriter
{
	private readonly IPathHelper _pathHelper;
	private readonly IInternalSerializerFactory _internalSerializerFactory;

	public ConfigWriter(IPathHelper pathHelper, IInternalSerializerFactory internalSerializerFactory)
	{
		_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
		_internalSerializerFactory = internalSerializerFactory ?? throw new ArgumentNullException(nameof(internalSerializerFactory));
	}

	/// <inheritdoc />
	public Task SaveProjectConfigAsync(IConfiguration configuration, CancellationToken cancellationToken = default)
	{
		return SaveProjectConfigAsync(
			configuration,
			_pathHelper.BuildFilePath(
				_pathHelper.GetExecutionBaseDirectory(),
				Defaults.DingoConfigFilename,
				Defaults.DingoConfigExtension
			),
			cancellationToken
		);
	}

	/// <inheritdoc />
	public async Task SaveProjectConfigAsync(IConfiguration configuration, string configPath, CancellationToken cancellationToken = default)
	{
		if (!Path.IsPathRooted(configPath))
		{
			configPath = _pathHelper.GetAbsolutePathFromRelative(configPath);
		}

		var internalSerializer = _internalSerializerFactory.CreateInternalSerializer(configPath);

		var stringBuilder = new StringBuilder();
		stringBuilder.Append(internalSerializer.Serialize(configuration));

		await using (var streamWriter = new StreamWriter(configPath))
		{
			await streamWriter.WriteAsync(stringBuilder, cancellationToken);
		}
	}
}