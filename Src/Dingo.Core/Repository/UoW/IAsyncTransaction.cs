namespace Dingo.Core.Repository.UoW;

public interface IAsyncTransaction
{
	ValueTask BeginAsync(CancellationToken ct = default);
	ValueTask CommitAsync(CancellationToken ct = default);
	ValueTask RollbackAsync(CancellationToken ct = default);
}
