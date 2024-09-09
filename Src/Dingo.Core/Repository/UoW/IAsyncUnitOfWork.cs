using System.Data.Common;

namespace Dingo.Core.Repository.UoW;

public interface IAsyncUnitOfWork: IAsyncDisposable, IAsyncTransaction, IUnitOfWorkBase
{
}
