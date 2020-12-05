using Dingo.Core.Constants;
using Dingo.Core.Extensions;
using System.Text.Json;

namespace Dingo.Core.Serializers
{
	/// <summary> Wrapper around JSON serializer </summary>
	internal class JsonInternalSerializer : IInternalSerializer
	{
		public string DefaultFileExtension => FileExtension.Json;

		/// <inheritdoc />
		public T Deserialize<T>(string contents)
		{
			return JsonSerializer.Deserialize<T>(contents);
		}

		/// <inheritdoc />
		public string Serialize<T>(T data)
		{
			var options = new JsonSerializerOptions
			{
				IgnoreNullValues = true,
				WriteIndented = true,
			};

			var serializedObject = JsonSerializer.Serialize(data, options);

			return serializedObject.ToUnixEol();
		}
	}
}