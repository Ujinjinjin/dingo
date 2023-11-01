using System.Data;

namespace Dingo.Core.Repository.Command;

public record Command(string Sql, object? Param = null, CommandType? CommandType = null);
