using System.Data;

namespace Dingo.Core.Repository;

public interface IConnectionFactory
{
	IDbConnection Create();
}
