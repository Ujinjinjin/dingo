using Dingo.Core.Models;

namespace Dingo.Core.Extensions
{
	/// <summary> Collection of extensions for <see cref="MessageType"/> </summary>
	public static class MessageTypeExtensions
	{
		/// <summary> Add prefix to message depending on message type </summary>
		/// <param name="source">Message type</param>
		/// <param name="message">Message</param>
		/// <returns>Message with prefix</returns>
		public static string AddPrefixToMessage(this MessageType source, string message)
		{
			return source switch
			{
				MessageType.Warning => $"Warning! {message}",
				MessageType.Error => $"Error! {message}",
				_ => message,
			};
		}
		
		/// <summary> Convert message type to text style </summary>
		/// <param name="source">Message type</param>
		/// <returns>Text style</returns>
		public static TextStyle ToTextStyle(this MessageType source)
		{
			return source switch
			{
				MessageType.Warning => TextStyle.Warning,
				MessageType.Info => TextStyle.Info,
				MessageType.Error => TextStyle.Error,
				MessageType.Success => TextStyle.Success,
				_ => TextStyle.Plain,
			};
		}
	}
}
