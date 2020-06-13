using Dingo.Cli.Constants;
using Dingo.Cli.Factories;
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
		private readonly IInternalSerializerFactory _internalSerializerFactory;
		
		public ConfigSaver(IPathHelper pathHelper, IInternalSerializerFactory internalSerializerFactory)
		{
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
			_internalSerializerFactory = internalSerializerFactory ?? throw new ArgumentNullException(nameof(internalSerializerFactory));
		}

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

		public async Task SaveProjectConfigAsync(IConfiguration configuration, string configPath, CancellationToken cancellationToken = default)
		{
			var internalSerializer = _internalSerializerFactory.CreateInternalSerializer(configPath);
			
			if (!File.Exists(configPath))
			{
				File.Create(configPath);
			}

			var fileContents = internalSerializer.Serialize(configuration);

			await File.WriteAllTextAsync(configPath, fileContents, cancellationToken);
		}
	}
}