namespace Dingo.Abstractions
{
	/// <summary> Command stack type </summary>
	public enum StackType
	{
		/// <summary> Unspecified stack type </summary>
		Unspecified = 0,

		/// <summary> Command nested from upper level command </summary>
		Nested = 1,

		/// <summary> Command embedded into upper command level </summary>
		Embedded = 2,
		
		/// <summary> Command hidden  </summary>
		Hidden = 3,
	}
}
