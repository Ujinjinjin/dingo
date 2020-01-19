using Dingo.Abstractions.Config;
using Dingo.Abstractions.Factories;
using Dingo.Core.Config;
using Dingo.Core.Models;
using System;
using YamlDotNet.Serialization;

namespace Dingo.Core.Factories
{
	public class ConfigFactory : IConfigFactory
	{
		private readonly IDeserializer _deserializer;
		private readonly ISerializer _serializer;

		public ConfigFactory(IDeserializer deserializer, ISerializer serializer)
		{
			_deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
			_serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
		}

		public IGlobalConfig LoadGlobalConfig()
		{
			return new GlobalConfig(_deserializer, _serializer);
		}

		public IProjectConfig LoadProjectConfig()
		{
			return new ProjectConfig(_deserializer, _serializer);
		}
	}
}
