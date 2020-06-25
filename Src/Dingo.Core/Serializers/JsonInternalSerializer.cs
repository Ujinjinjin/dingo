using Dingo.Core.Extensions;
using System.Text.Json;

namespace Dingo.Core.Serializers
{
	internal class JsonInternalSerializer : IInternalSerializer
	{
		public string DefaultFileExtension => ".json";

		public string Serialize<T>(T data)
		{
			var options = new JsonSerializerOptions
			{
				IgnoreNullValues = true
			};
			
			var serializedObject = JsonSerializer.Serialize(data, options);

			return serializedObject.ToUnixEol();
		}

		public T Deserialize<T>(string contents)
		{
			return JsonSerializer.Deserialize<T>(contents);
		}
	}
}