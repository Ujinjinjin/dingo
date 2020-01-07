namespace Dingo.Abstractions
{
	/// <summary> Command stack type </summary>
	public enum StackType
	{
		/// <summary> Unspecified stack type </summary>
		Unspecified = 0,

		/// <summary> Nested stack type </summary>
		Nested = 1,

		/// <summary> Embedded stack type </summary>
		Embedded = 2,
	}
}
