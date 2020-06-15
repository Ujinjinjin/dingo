using Dingo.Core.Constants;
using Dingo.Core.Factories;
using Dingo.Core.Operations;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Core.Config
{
	internal class ConfigLoader : IConfigLoader
	{
		private readonly IPathHelper _pathHelper;
		private readonly IInternalSerializerFactory _internalSerializerFactory;

		public ConfigLoader(IPathHelper pathHelper, IInternalSerializerFactory internalSerializerFactory)
		{
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
			_internalSerializerFactory = internalSerializerFactory ?? throw new ArgumentNullException(nameof(internalSerializerFactory));
		}

		public Task<IConfiguration> LoadProjectConfigAsync(CancellationToken cancellationToken = default)
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

		public async Task<IConfiguration> LoadProjectConfigAsync(string configPath, CancellationToken cancellationToken = default)
		{
			var internalSerializer = _internalSerializerFactory.CreateInternalSerializer(configPath);
			
			if (!File.Exists(configPath))
			{
				throw new FileNotFoundException("Error", configPath);
			}

			var fileContents = await File.ReadAllTextAsync(configPath, cancellationToken);

			return internalSerializer.Deserialize<ProjectConfiguration>(fileContents);
		}
	}
}