using Dingo.Core.Abstractions;
using Sharprompt;
using System;
using System.Collections.Generic;

namespace Dingo.Cli.Implementors
{
	/// <summary> CLI interactions with user </summary>
	public sealed class CliPrompt : IPrompt
	{
		/// <inheritdoc />
		public T Choose<T>(string message, IList<T> choiceList, Func<T, string> textSelector = null)
		{
			return Prompt.Select(message, choiceList, textSelector: textSelector);
		}

		/// <inheritdoc />
		public bool Confirm(string message, bool? defaultValue = null)
		{
			return Prompt.Confirm(message, defaultValue);
		}
	}
}
