using System.Diagnostics.CodeAnalysis;

namespace Dingo.Core.Repository.UoW;

public interface IUnitOfWorkFactory
{
	Task<IAsyncUnitOfWork> CreateAsync();
	bool TryGet([MaybeNullWhen(false)] out IUnitOfWorkBase unitOfWork);
}
