using System.Data;
using System.Data.Common;

namespace Dingo.Core.Repository;

public interface IConnectionFactory
{
	DbConnection Create();
}
