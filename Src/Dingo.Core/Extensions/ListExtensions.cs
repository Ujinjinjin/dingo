using System;
using System.Collections.Generic;

namespace Dingo.Core.Extensions
{
	/// <summary> Collection of extensions for <see cref="IList{T}"/> </summary>
	internal static class ListExtensions
	{
		/// <summary> Get item from source list at specified index </summary>
		/// <param name="source">Source list of items</param>
		/// <param name="index">Index</param>
		/// <typeparam name="T">The type of items in the list</typeparam>
		/// <returns>Item at specified index</returns>
		public static T GetItem<T>(this IList<T> source, Index index)
		{
			return source[index];
		}

		/// <summary> Get random item from source list </summary>
		/// <param name="source">Source list of items</param>
		/// <typeparam name="T">The type of items in the list</typeparam>
		/// <returns>Random item from list</returns>
		public static T GetRandom<T>(this IList<T> source)
		{
			return source[new Random().Next(0, source.Count)];
		}

		/// <summary> Select sequence from list of item beginning at startIndex and ending at endIndex </summary>
		/// <param name="source">Source list of items</param>
		/// <param name="startIndex">Start index</param>
		/// <param name="endIndex">End index</param>
		/// <typeparam name="T">The type of items in the list</typeparam>
		/// <returns>Sequence of items</returns>
		public static IList<T> Sequence<T>(this IList<T> source, Index startIndex, Index endIndex)
		{
			if (endIndex.IsFromEnd)
			{
				endIndex = source.Count - endIndex.Value;
			}

			var targetLength = endIndex.Value - startIndex.Value;
			var target = new T[targetLength];

			for (var i = 0; i < targetLength; i++)
			{
				target[i] = source[startIndex.Value + i];
			}

			return target;
		}

		/// <summary> Select sequence from list of item beginning at startIndex and ending with last item </summary>
		/// <param name="source">Source list of items</param>
		/// <param name="startIndex">Start index</param>
		/// <typeparam name="T">The type of items in the list</typeparam>
		/// <returns>Sequence of items</returns>
		public static IList<T> SequenceFrom<T>(this IList<T> source, Index startIndex)
		{
			return Sequence(source, startIndex, ^1);
		}

		/// <summary> Select sequence from list of item beginning with first item and ending at endIndex </summary>
		/// <param name="source">Source list of items</param>
		/// <param name="endIndex">End index</param>
		/// <typeparam name="T">The type of items in the list</typeparam>
		/// <returns>Sequence of items</returns>
		public static IList<T> SequenceTo<T>(this IList<T> source, Index endIndex)
		{
			return Sequence(source, 0, endIndex);
		}
	}
}
