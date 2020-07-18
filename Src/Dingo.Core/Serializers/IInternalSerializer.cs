namespace Dingo.Core.Serializers
{
	/// <summary> Wrapper around serializers </summary>
	public interface IInternalSerializer
	{
		/// <summary> File extension associated with serializer </summary>
		string DefaultFileExtension { get; }
		
		/// <summary> Deserialize given string into object specified type </summary>
		/// <param name="contents">Serialized object</param>
		/// <typeparam name="T">Type of the data</typeparam>
		/// <returns>Object deserialized from given string</returns>
		T Deserialize<T>(string contents);
		
		/// <summary> Serialize given data to string </summary>
		/// <param name="data">Data to serialize</param>
		/// <typeparam name="T">Type of the data</typeparam>
		/// <returns>Serialized string</returns>
		string Serialize<T>(T data);
	}
}