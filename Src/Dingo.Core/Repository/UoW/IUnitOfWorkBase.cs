using System.Data.Common;

namespace Dingo.Core.Repository.UoW;

public interface IUnitOfWorkBase
{
	Guid Id { get; }
	DbConnection Connection { get; }
	DbTransaction? Transaction { get; }
}
