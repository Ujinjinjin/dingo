using Dingo.Cli.Operations;
using Dingo.Cli.Serializers;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Cli.Config
{
	internal class ConfigSaver : IConfigSaver
	{
		private readonly IPathHelper _pathHelper;
		private readonly IInternalSerializer _internalSerializer;
		
		public ConfigSaver(IPathHelper pathHelper, IInternalSerializer internalSerializer)
		{
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
			_internalSerializer = internalSerializer ?? throw new ArgumentNullException(nameof(internalSerializer));
		}

		public Task SaveProjectConfigAsync(IConfiguration configuration, CancellationToken cancellationToken = default)
		{
			return SaveProjectConfigAsync(
				configuration,
				_pathHelper.BuildFilePath(
					_pathHelper.GetExecutionBaseDirectory(),
					Constants.Constants.DefaultDingoConfigFilename,
					_internalSerializer.DefaultFileExtension
				),
				cancellationToken
			);
		}

		public async Task SaveProjectConfigAsync(IConfiguration configuration, string configPath, CancellationToken cancellationToken = default)
		{
			if (!File.Exists(configPath))
			{
				File.Create(configPath);
			}

			var fileContents = _internalSerializer.Serialize(configuration);

			await File.WriteAllTextAsync(configPath, fileContents, cancellationToken);
		}
	}
}