using System;

namespace Dingo.Core.Exceptions
{
	public class SubCommandNotSpecifiedException : Exception
	{
		public SubCommandNotSpecifiedException()
		{
		}

		public SubCommandNotSpecifiedException(string message)
			: base(message)
		{
		}

		public SubCommandNotSpecifiedException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
