namespace Dingo.Core.Extensions;

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

	/// <summary> Select sequence from list of item beginning at startIndex and ending at endIndex </summary>
	/// <param name="source">Source list of items</param>
	/// <param name="range">Range of item indices</param>
	/// <typeparam name="T">The type of items in the list</typeparam>
	/// <returns>Sequence of items</returns>
	public static IList<T> Sequence<T>(this IList<T> source, Range range)
	{
		if (range.Start.Value >= source.Count || range.End.Value >= source.Count)
		{
			throw new IndexOutOfRangeException();
		}

		var start = range.Start.IsFromEnd
			? source.Count - range.Start.Value
			: range.Start.Value;
		var end = range.End.IsFromEnd
			? source.Count - range.End.Value
			: range.End.Value;

		return start < end
			? source.ForwardSequence(start, end)
			: source.ReverseSequence(start, end);
	}

	private static IList<T> ForwardSequence<T>(this IList<T> source, int start, int end)
	{
		var length = end - start;
		var target = new T[length];

		for (var i = 0; i < length; i++)
		{
			target[i] = source[i + start];
		}

		return target;
	}

	private static IList<T> ReverseSequence<T>(this IList<T> source, int start, int end)
	{
		var length = start - end;
		var target = new T[length];

		for (var i = 0; i < length; i++)
		{
			target[i] = source[start - 1 - i];
		}

		return target;
	}
}
