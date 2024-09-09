using System.Data;
using System.Data.Common;
using Dapper;
using Dingo.Core.Repository.Command;

namespace Dingo.Core.Extensions;

internal static class DbConnectionExtensions
{
	public static Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection cnn, Command command, DbTransaction? transaction)
	{
		return cnn.QueryAsync<T>(command.Sql, command.Param, commandType: command.CommandType, transaction: transaction);
	}

	public static Task<int> ExecuteAsync(this IDbConnection cnn, Command command, DbTransaction? transaction)
	{
		return cnn.ExecuteAsync(command.Sql, command.Param, commandType: command.CommandType, transaction: transaction);
	}
}
