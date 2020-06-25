namespace Dingo.Core.Abstractions
{
	/// <summary> Abstraction over interactions with user </summary>
	public interface IPrompt
	{
		/// <summary> Show confiramation message to user and return his choice </summary>
		/// <param name="message">Message shown to user</param>
		/// <param name="defaultValue">Default value (null if none)</param>
		/// <returns>User's choice as bool</returns>
		bool Confirm(string message, bool? defaultValue = null);
	}
}
