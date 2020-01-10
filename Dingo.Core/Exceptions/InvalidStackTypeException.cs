using System;

namespace Dingo.Core.Exceptions
{
	public class InvalidStackTypeException : Exception
	{
		public InvalidStackTypeException()
		{
		}

		public InvalidStackTypeException(string message)
			: base(message)
		{
		}

		public InvalidStackTypeException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
