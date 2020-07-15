using Dingo.Core.Extensions;
using System.Collections.Generic;

namespace Dingo.UnitTests.Base
{
	public class UnitTestsBase
	{
		protected IList<int> CreateIntArray(int length)
		{
			if (length < 0)
			{
				length = length.Negate();
			}
			
			var array = new int[length];
			
			for (var i = 0; i < length; i++)
			{
				array[i] = i;
			}

			return array;
		}
	}
}
