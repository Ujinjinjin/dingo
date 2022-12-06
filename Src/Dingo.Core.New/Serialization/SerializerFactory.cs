using Dingo.Core.Extensions;

namespace Dingo.Core.Serialization;

/// <inheritdoc />
public sealed class SerializerFactory : ISerializerFactory
{
	/// <inheritdoc />
	public ISerializer Create(string filename)
	{
		if (string.IsNullOrWhiteSpace(filename))
		{
			throw new ArgumentNullException(nameof(filename));
		}

		var fileExtension = filename
			.Split('.')
			.GetItem(^1);

		switch (fileExtension)
		{
			case FileExtension.Json:
				return new JsonSerializer();
			case FileExtension.Yaml:
			case FileExtension.Yml:
				return new YamlSerializer();
			default:
				throw new ArgumentOutOfRangeException(nameof(filename));
		}
	}
}
