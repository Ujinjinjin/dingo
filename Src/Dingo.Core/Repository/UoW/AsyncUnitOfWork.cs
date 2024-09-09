using System.Data.Common;

namespace Dingo.Core.Repository.UoW;

public class AsyncUnitOfWork : IAsyncUnitOfWork
{
	public Guid Id { get; }
	public DbConnection Connection { get; }
	public DbTransaction? Transaction { get; private set; }

	private readonly Action _onDisposed;

	internal AsyncUnitOfWork(DbConnection connection, Action onDisposed)
	{
		Id = Guid.NewGuid();
		Connection = connection;
		_onDisposed = onDisposed;
	}

	public async ValueTask BeginAsync(CancellationToken ct = default)
	{
		Transaction = await Connection.BeginTransactionAsync(ct);
	}

	public async ValueTask CommitAsync(CancellationToken ct = default)
	{
		if (Transaction is not null)
		{
			await Transaction.CommitAsync(ct);
			await DisposeTransactionAsync();
		}
	}

	public async ValueTask RollbackAsync(CancellationToken ct = default)
	{
		if (Transaction is not null)
		{
			await Transaction.RollbackAsync(ct);
			await DisposeTransactionAsync();
		}
	}

	public async ValueTask DisposeAsync()
	{
		_onDisposed();
		await DisposeTransactionAsync();
		await DisposeConnectionAsync();
		GC.SuppressFinalize(this);
	}

	private async ValueTask DisposeConnectionAsync()
	{
		await Connection.CloseAsync();
		await Connection.DisposeAsync();
	}

	private async ValueTask DisposeTransactionAsync()
	{
		if (Transaction is not null)
		{
			await Transaction.DisposeAsync();
		}

		Transaction = null;
	}
}
