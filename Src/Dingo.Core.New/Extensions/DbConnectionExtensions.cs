using System.Data;
using Dapper;
using Dingo.Core.Repository.Command;

namespace Dingo.Core.Extensions;

internal static class DbConnectionExtensions
{
	public static Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection cnn, Command command)
	{
		return cnn.QueryAsync<T>(command.Sql, command.Param, commandType: command.CommandType);
	}
}
