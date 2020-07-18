using Dingo.Core.Constants;
using Dingo.Core.Facades;
using Dingo.Core.Factories;
using Dingo.Core.Helpers;
using Dingo.Core.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Core.Config
{
	/// <inheritdoc />
	internal class ConfigLoader : IConfigLoader
	{
		private readonly IPathHelper _pathHelper;
		private readonly IFileFacade _fileFacade;
		private readonly IInternalSerializerFactory _internalSerializerFactory;

		public ConfigLoader(
			IPathHelper pathHelper,
			IFileFacade fileFacade,
			IInternalSerializerFactory internalSerializerFactory
		)
		{
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
			_fileFacade = fileFacade ?? throw new ArgumentNullException(nameof(fileFacade));
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
			
			if (!_fileFacade.Exists(configPath))
			{
				return new LoadConfigResult
				{
					Configuration = new ProjectConfiguration(),
					ConfigPath = null
				};
			}

			var fileContents = await _fileFacade.ReadAllTextAsync(configPath, cancellationToken);

			return new LoadConfigResult
			{
				Configuration = internalSerializer.Deserialize<ProjectConfiguration>(fileContents),
				ConfigPath = configPath
			};
		}
	}
}