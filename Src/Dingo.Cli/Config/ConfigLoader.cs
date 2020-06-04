using Dingo.Cli.Operations;
using Dingo.Cli.Serializers;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Cli.Config
{
	internal class ConfigLoader : IConfigLoader
	{
		private readonly IPathHelper _pathHelper;
		private readonly IInternalSerializer _internalSerializer;

		public ConfigLoader(IPathHelper pathHelper, IInternalSerializer internalSerializer)
		{
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
			_internalSerializer = internalSerializer ?? throw new ArgumentNullException(nameof(internalSerializer));
		}

		public Task<IConfiguration> LoadProjectConfigAsync(CancellationToken cancellationToken = default)
		{
			return LoadProjectConfigAsync(
				_pathHelper.BuildFilePath(
					_pathHelper.GetExecutionBaseDirectory(),
					Constants.Constants.DefaultDingoConfigFilename,
					_internalSerializer.DefaultFileExtension
				),
				cancellationToken
			);
		}

		public async Task<IConfiguration> LoadProjectConfigAsync(string configPath, CancellationToken cancellationToken = default)
		{
			if (!File.Exists(configPath))
			{
				throw new FileNotFoundException("Error", configPath);
			}

			var fileContents = await File.ReadAllTextAsync(configPath, cancellationToken);

			return _internalSerializer.Deserialize<ProjectConfiguration>(fileContents);
		}
	}
}