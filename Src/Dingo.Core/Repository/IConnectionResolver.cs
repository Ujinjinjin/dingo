using System.Data.Common;

namespace Dingo.Core.Repository;

public interface IConnectionResolver : IDisposable, IAsyncDisposable
{
	DbConnection Connection { get; }
	DbTransaction? Transaction { get; }
}
