using Dingo.Core.Abstractions;
using Sharprompt;
using System.Collections.Generic;

namespace Dingo.Cli.Implementors
{
	/// <summary> CLI interactions with user </summary>
	public class CliPrompt : IPrompt
	{
		/// <inheritdoc />
		public T Choose<T>(string message, IList<T> choiceList)
		{
			return Prompt.Select(message, choiceList);
		}
		
		/// <inheritdoc />
		public bool Confirm(string message, bool? defaultValue = null)
		{
			return Prompt.Confirm(message, defaultValue);
		}
	}
}
