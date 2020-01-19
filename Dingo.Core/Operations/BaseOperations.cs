using Dingo.Abstractions.Config;
using System;

namespace Dingo.Core.Operations
{
	public class BaseOperations
	{
		private readonly IGlobalConfig _globalConfig;
		private readonly IProjectConfig _projectConfig;

		protected BaseOperations(IGlobalConfig globalConfig, IProjectConfig projectConfig)
		{
			_globalConfig = globalConfig ?? throw new ArgumentNullException(nameof(globalConfig));
			_projectConfig = projectConfig ?? throw new ArgumentNullException(nameof(projectConfig));
		}
		
		protected virtual (IUpdatable, IDingoConfig) GetConfig(bool global)
		{
			var updater = global
				? (IUpdatable) _globalConfig
				: _projectConfig;

			var config = global
				? (IDingoConfig) _globalConfig
				: _projectConfig;

			return (updater, config);
		}
	}
}
