using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Dingo.Core.Extensions;

namespace Dingo.Core.Serialization;

/// <summary> Wrapper around JSON serializer </summary>
internal sealed class JsonSerializer : ISerializer
{
	/// <inheritdoc />
	public T Deserialize<T>(string contents)
	{
		return System.Text.Json.JsonSerializer.Deserialize<T>(contents) ?? throw new SerializationException();
	}

	/// <inheritdoc />
	public string Serialize<T>(T data)
	{
		var options = new JsonSerializerOptions
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			WriteIndented = true,
		};

		var serializedObject = System.Text.Json.JsonSerializer.Serialize(data, options);

		return serializedObject.ToUnixEol();
	}
}
