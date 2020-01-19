using Dingo.Abstractions.Config;
using Dingo.Core.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Dingo.Core.Config
{
	public abstract class BaseConfig
	{
		private const string ConfigFile = "config.yaml";

		private readonly IDeserializer _deserializer;
		private readonly ISerializer _serializer;

		protected BaseConfig(IDeserializer deserializer, ISerializer serializer)
		{
			_deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
			_serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
		}
		
		protected virtual async Task UpdateAsync(IDingoConfig config, string directory)
		{
			if (!Directory.Exists(directory))
			{
				Initialize(directory);
			}
			
			var configFilePath = GetConfigFilePath(directory);
			
			using (var configFile = new StreamWriter(configFilePath))
			{
				await configFile.WriteLineAsync(_serializer.Serialize(config));
			}
		}

		protected virtual void Initialize(string directory)
		{
			Directory.CreateDirectory(directory);
		}

		protected virtual async Task LoadAsync(IDingoConfig config, string directory)
		{
			var configFilePath = GetConfigFilePath(directory);
			
			if (!File.Exists(configFilePath))
			{
				return;
			}

			using (var configFile = new StreamReader(configFilePath))
			{
				var configData = await configFile.ReadToEndAsync();
				var configModel = _deserializer.Deserialize<ConfigModel>(configData);

				config.ConnectionString = configModel.ConnectionString;
				config.DatabaseEngine = configModel.DatabaseEngine;
			}
		}

		protected string GetConfigDirectory(string baseDirectory) => $"{baseDirectory}\\.dingo\\";
		private string GetConfigFilePath(string directory) => $"{directory}{ConfigFile}";
	}
}
