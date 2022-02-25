using LinqToDB;
using LinqToDB.Data;
using System.Collections.Generic;
using System.Data;

namespace Dingo.Core.Repository.DbConverters;

/// <summary> Base class for database contract converter implementations </summary>
internal abstract class DatabaseContractConverterBase
{
	/// <summary> Create table data parameter using given row mappers </summary>
	/// <param name="parameterName">Parameter name in database</param>
	/// <param name="parameterType">Parameter type name in database</param>
	/// <param name="sourceValueList">Source list of values</param>
	/// <param name="rowMapList">List of row mappers</param>
	/// <typeparam name="T">Database contract type</typeparam>
	/// <returns>Table data parameter</returns>
	protected DataParameter ToDataParameter<T>(
		string parameterName,
		string parameterType,
		IList<T> sourceValueList,
		IList<RowMap<T>> rowMapList
	)
	{
		var table = new DataTable(parameterType);

		foreach (var rowMap in rowMapList)
		{
			table.Columns.Add(rowMap.Column);
		}
			
		var rowValues = new object[rowMapList.Count];
		foreach (var value in sourceValueList)
		{
			var i = 0;
			foreach (var mapping in rowMapList)
			{
				rowValues[i] = mapping.Extractor(value);
				i++;
			}

			table.Rows.Add(rowValues);
		}

		return new DataParameter(parameterName, table, DataType.Udt);
	}
}