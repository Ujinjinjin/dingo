using Dingo.Core.Extensions;
using Dingo.Core.Repository.UoW;

namespace Dingo.Core.Repository;

internal class ConnectionResolverFactory : IConnectionResolverFactory
{
	private readonly IUnitOfWorkFactory _unitOfWorkFactory;
	private readonly IConnectionFactory _connectionFactory;

	public ConnectionResolverFactory(
		IUnitOfWorkFactory unitOfWorkFactory,
		IConnectionFactory connectionFactory
	)
	{
		_unitOfWorkFactory = unitOfWorkFactory.Required(nameof(unitOfWorkFactory));
		_connectionFactory = connectionFactory.Required(nameof(connectionFactory));
	}

	public IConnectionResolver Create()
	{
		return new ConnectionResolver(_unitOfWorkFactory, _connectionFactory);
	}
}
