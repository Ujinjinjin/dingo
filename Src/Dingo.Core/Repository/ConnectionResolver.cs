using System.Data.Common;
using Dingo.Core.Repository.UoW;

namespace Dingo.Core.Repository;

internal sealed class ConnectionResolver : IConnectionResolver
{
	private readonly bool _isUnitOfWork;
	public DbConnection Connection { get; }

	public ConnectionResolver(IUnitOfWorkFactory unitOfWorkFactory, IConnectionFactory connectionFactory)
	{
		_isUnitOfWork = unitOfWorkFactory.TryGet(out var unitOfWork);
		Connection = unitOfWork is not null ? unitOfWork.Connection : connectionFactory.Create();
	}

	public void Dispose()
	{
		if (_isUnitOfWork) return;

		Connection.Close();
		Connection.Dispose();
	}

	public async ValueTask DisposeAsync()
	{
		if (_isUnitOfWork) return;

		await Connection.CloseAsync();
		await Connection.DisposeAsync();
	}
}
