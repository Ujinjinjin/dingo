namespace Dingo.Core.Serialization;

/// <summary> Internal serializer factory </summary>
public interface ISerializerFactory
{
	/// <summary> Create internal serializer suitable for specified file </summary>
	/// <param name="filename">Filename</param>
	/// <returns>Internal serializer</returns>
	ISerializer Create(string filename);
}
