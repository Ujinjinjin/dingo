namespace Dingo.Core.Repository.DbConverters;

/// <summary> Provides data structure fields to table columns mapping </summary>
/// <typeparam name="T">Type of data structure or a class</typeparam>
internal sealed class RowMap<T>
{
	/// <summary> Database table or type column name </summary>
	public readonly string Column;

	/// <summary> Extractor of a field value </summary>
	public readonly Func<T, object> Extractor;

	/// <summary> Create <see cref="RowMap{T}"/> </summary>
	/// <param name="column"><see cref="Column"/></param>
	/// <param name="extractor"><see cref="Extractor"/></param>
	public RowMap(string column, Func<T, object> extractor)
	{
		Column = column;
		Extractor = extractor;
	}
}