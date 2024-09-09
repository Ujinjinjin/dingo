using System.Diagnostics.CodeAnalysis;
using Dingo.Core.Exceptions;
using Dingo.Core.Extensions;

namespace Dingo.Core.Repository.UoW;

internal class UnitOfWorkFactory : IUnitOfWorkFactory
{
	private readonly IConnectionFactory _connectionFactory;
	private readonly Stack<IAsyncUnitOfWork> _units = new();

	public UnitOfWorkFactory(IConnectionFactory connectionFactory)
	{
		_connectionFactory = connectionFactory.Required(nameof(connectionFactory));
	}

	public async Task<IAsyncUnitOfWork> CreateAsync()
	{
		var connection = _connectionFactory.Create();
		await connection.OpenAsync();

		_units.Push(new AsyncUnitOfWork(connection, () => _units.Pop()));

		return _units.Peek();
	}

	/// <summary>
	/// In a multithreaded environment this approach won't work, but considering that dingo is a lightweight
	/// cli tool not using multiple threads, we can safely assume that there are no concurrency
	/// </summary>
	public bool TryGet([MaybeNullWhen(false)] out IUnitOfWorkBase unitOfWork)
	{
		var result = _units.TryPeek(out var asyncUnitOfWork);
		unitOfWork = asyncUnitOfWork;
		return result;
	}
}
