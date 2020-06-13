using Dingo.Cli.Serializers;

namespace Dingo.Cli.Factories
{
	public interface IInternalSerializerFactory
	{
		IInternalSerializer CreateInternalSerializer(string filename);
	}
}