using Dingo.Core.Serializers;

namespace Dingo.Core.Factories
{
	/// <summary> Internal serializer factory </summary>
	public interface IInternalSerializerFactory
	{
		/// <summary> Create internal serializer suitable for specified file </summary>
		/// <param name="filename">Filename</param>
		/// <returns>Internal serializer</returns>
		IInternalSerializer CreateInternalSerializer(string filename);
	}
}