using System.Collections.Generic;

namespace Dingo.Cli.Extensions
{
	internal static class ListExtensions
	{
		public static IList<T> Sequence<T>(this IList<T> source, int startIndex, int endIndex)
		{
			if (endIndex < 0)
			{
				endIndex = source.Count - endIndex.Negate();
			}
			
			var targetLength = endIndex - startIndex;
			var target = new T[targetLength];

			for (var i = 0; i < targetLength; i++)
			{
				target[i] = source[startIndex + i];
			}

			return target;
		}
		
		public static IList<T> SequenceFrom<T>(this IList<T> source, int startIndex)
		{
			return Sequence(source, startIndex, -1);
		}
		
		public static IList<T> SequenceTo<T>(this IList<T> source, int endIndex)
		{
			return Sequence(source, 0, endIndex);
		}
	}
}
