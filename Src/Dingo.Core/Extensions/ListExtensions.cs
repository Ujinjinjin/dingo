using System.Collections.Generic;

namespace Dingo.Core.Extensions
{
	/// <summary> Collection of extensions for <see cref="IList{T}"/> </summary>
	internal static class ListExtensions
	{
		/// <summary> Select sequence from list of item beginning at startIndex and ending at endIndex </summary>
		/// <param name="source">Source list of items</param>
		/// <param name="startIndex">Start index</param>
		/// <param name="endIndex">End index</param>
		/// <typeparam name="T">The type of items in the list</typeparam>
		/// <returns>Sequence of items</returns>
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
		
		/// <summary> Select sequence from list of item beginning at startIndex and ending with last item </summary>
		/// <param name="source">Source list of items</param>
		/// <param name="startIndex">Start index</param>
		/// <typeparam name="T">The type of items in the list</typeparam>
		/// <returns>Sequence of items</returns>
		public static IList<T> SequenceFrom<T>(this IList<T> source, int startIndex)
		{
			return Sequence(source, startIndex, -1);
		}
		
		/// <summary> Select sequence from list of item beginning with first item and ending at endIndex </summary>
		/// <param name="source">Source list of items</param>
		/// <param name="endIndex">End index</param>
		/// <typeparam name="T">The type of items in the list</typeparam>
		/// <returns>Sequence of items</returns>
		public static IList<T> SequenceTo<T>(this IList<T> source, int endIndex)
		{
			return Sequence(source, 0, endIndex);
		}
		
		/// <summary> Get item from source list at specified index with support of negative index where -1=last </summary>
		/// <param name="source">Source list of items</param>
		/// <param name="index">Index</param>
		/// <typeparam name="T">The type of items in the list</typeparam>
		/// <returns>Item at specified index</returns>
		public static T GetItem<T>(this IList<T> source, int index)
		{
			if (index < 0)
			{
				index = source.Count - index.Negate();
			}
			
			return source[index];
		}
	}
}
