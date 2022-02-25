namespace Dingo.Core.Abstractions;

/// <summary> Abstraction over interactions with user </summary>
public interface IPrompt
{
	/// <summary> Show list of items to user and return his choice </summary>
	/// <param name="message">Message shown to user</param>
	/// <param name="choiceList">List of choices shown to user</param>
	/// <param name="textSelector">Value selector</param>
	/// <typeparam name="T">Type of choice item</typeparam>
	/// <returns>User's choice</returns>
	T Choose<T>(string message, IList<T> choiceList, Func<T, string> textSelector = null);

	/// <summary> Show confirmation message to user and return his choice </summary>
	/// <param name="message">Message shown to user</param>
	/// <param name="defaultValue">Default value (null if none)</param>
	/// <returns>User's choice as bool</returns>
	bool Confirm(string message, bool? defaultValue = null);
}