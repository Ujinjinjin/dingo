using Dingo.Core.Extensions;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Yaml;
using System.Yaml.Serialization;

namespace Dingo.Core.Serializers
{
	internal class YamlInternalSerializer : IInternalSerializer
	{
		public string DefaultFileExtension => ".yml";

		public string Serialize<T>(T data)
		{
			var config = new YamlConfig
			{
				DontUseVerbatimTag = true,
				OmitTagForRootNode = true
			};
			var serializer = new YamlSerializer(config);

			var dirtySerializedArray = serializer.Serialize(data).Split("\n");
			var cleanSerializedList = new List<string>();
			
			for (var i = 0; i < dirtySerializedArray.Length; i++)
			{
				if (dirtySerializedArray[i].NotContains(": null"))
				{
					cleanSerializedList.Add(dirtySerializedArray[i]);
				}
			}

			var serializedObject = string.Join("\n", cleanSerializedList.Sequence(2, -2));
			
			return serializedObject.ToUnixEol();
		}

		public T Deserialize<T>(string contents)
		{
			var serializer = new YamlSerializer();
			var deserializationResult = serializer.Deserialize(contents, typeof(T));
			
			if (deserializationResult.Length != 1)
			{
				throw new SerializationException();
			}

			return (T) deserializationResult[0];
		}
	}
}