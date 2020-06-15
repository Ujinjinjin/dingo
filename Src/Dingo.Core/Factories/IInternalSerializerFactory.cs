using Dingo.Core.Serializers;

namespace Dingo.Core.Factories
{
	public interface IInternalSerializerFactory
	{
		IInternalSerializer CreateInternalSerializer(string filename);
	}
}