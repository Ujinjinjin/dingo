namespace Dingo.Core.Serializers
{
	public interface IInternalSerializer
	{
		string DefaultFileExtension { get; }
		string Serialize<T>(T data);
		T Deserialize<T>(string contents);
	}
}