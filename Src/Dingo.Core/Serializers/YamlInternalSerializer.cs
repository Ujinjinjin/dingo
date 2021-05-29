using Dingo.Core.Constants;
using Dingo.Core.Extensions;
using Jarl.Yaml;
using Jarl.Yaml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Dingo.Core.Serializers
{
	/// <summary> Wrapper around YAML serializer </summary>
	internal sealed class YamlInternalSerializer : IInternalSerializer
	{
		public string DefaultFileExtension => FileExtension.Yml;

		/// <inheritdoc />
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

		/// <inheritdoc />
		public string Serialize<T>(T data)
		{
			var config = new YamlConfig
			{
				DoNotUseVerbatimTag = true,
				OmitTagForRootNode = true,
				LineBreakForOutput = "\n"
			};
			var serializer = new YamlSerializer(config);

			var dirtySerializedArray = serializer.Serialize(data)
				.ToUnixEol()
				.Split("\n");
			var cleanSerializedList = new List<string>();

			for (var i = 0; i < dirtySerializedArray.Length; i++)
			{
				if (dirtySerializedArray[i].NotContains(": null"))
				{
					cleanSerializedList.Add(dirtySerializedArray[i]);
				}
			}

			cleanSerializedList = cleanSerializedList.Sequence(2, ^2)
				.OrderBy(x => x)
				.ToList();

			var serializedObject = string.Join("\n", cleanSerializedList);

			return serializedObject.ToUnixEol();
		}
	}
}