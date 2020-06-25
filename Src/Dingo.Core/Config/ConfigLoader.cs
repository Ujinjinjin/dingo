﻿using Dingo.Core.Constants;
using Dingo.Core.Factories;
using Dingo.Core.Models;
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

		public async Task<LoadConfigResult> LoadProjectConfigAsync(string configPath, CancellationToken cancellationToken = default)
		{
			if (!Path.IsPathRooted(configPath))
			{
				configPath = _pathHelper.GetAbsolutePathFromRelative(configPath);
			}

			var internalSerializer = _internalSerializerFactory.CreateInternalSerializer(configPath);
			
			if (!File.Exists(configPath))
			{
				return new LoadConfigResult
				{
					Configuration = new ProjectConfiguration(),
					ConfigPath = null
				};
			}

			var fileContents = await File.ReadAllTextAsync(configPath, cancellationToken);

			return new LoadConfigResult
			{
				Configuration = internalSerializer.Deserialize<ProjectConfiguration>(fileContents),
				ConfigPath = configPath
			};
		}
	}
}