using Dingo.Abstractions;
using Dingo.Abstractions.Config;
using System;
using System.Data;

namespace Dingo.Core.Config
{
	public class DingoConfig : IDingoConfig
	{
		private readonly IGlobalConfig _globalConfig;
		private readonly IProjectConfig _projectConfig;

		public DingoConfig(IGlobalConfig globalConfig, IProjectConfig projectConfig)
		{
			_globalConfig = globalConfig ?? throw new ArgumentNullException(nameof(globalConfig));
			_projectConfig = projectConfig ?? throw new ArgumentNullException(nameof(projectConfig));
		}

		public string ConnectionString
		{
			get => _projectConfig.ConnectionString ?? _globalConfig.ConnectionString;
			set => throw new ReadOnlyException();
		}

		public DatabaseEngine? DatabaseEngine
		{
			get => _projectConfig.DatabaseEngine ?? _globalConfig.DatabaseEngine;
			set => throw new ReadOnlyException();
		}

		public string DingoDirectory
		{
			get => throw new NotSupportedException();
			set => throw new NotSupportedException();
		}
	}
}
