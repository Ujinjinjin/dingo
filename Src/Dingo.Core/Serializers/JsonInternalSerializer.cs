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
			return JsonSerializer.Serialize(data, options);
		}

		public T Deserialize<T>(string contents)
		{
			return JsonSerializer.Deserialize<T>(contents);
		}
	}
}