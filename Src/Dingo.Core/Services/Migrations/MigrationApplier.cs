using Dingo.Core.Exceptions;
using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Dingo.Core.Repository;

namespace Dingo.Core.Services.Migrations;

internal sealed class MigrationApplier : IMigrationApplier
{
	private readonly IRepository _repository;

	public MigrationApplier(IRepository repository)
	{
		_repository = repository.Required(nameof(repository));
	}

	public async Task ApplyAndRegisterAsync(Migration migration, int patchNumber, CancellationToken ct = default)
	{
		if (!await IsDatabaseAvailableAsync())
		{
			throw new ConnectionNotEstablishedException();
		}

		await _repository.ExecuteAsync(migration.Command.Up, ct);
		await _repository.RegisterMigrationAsync(migration, patchNumber, ct);
	}

	public async Task ApplyAsync(Migration migration, CancellationToken ct = default)
	{
		if (!await IsDatabaseAvailableAsync())
		{
			throw new ConnectionNotEstablishedException();
		}

		await _repository.ExecuteAsync(migration.Command.Up, ct);
	}

	public async Task RegisterAsync(Migration migration, int patchNumber, CancellationToken ct = default)
	{
		if (!await IsDatabaseAvailableAsync())
		{
			throw new ConnectionNotEstablishedException();
		}

		await _repository.RegisterMigrationAsync(migration, patchNumber, ct);
	}

	public async Task RevertAsync(Migration migration, CancellationToken ct = default)
	{
		if (!await IsDatabaseAvailableAsync())
		{
			throw new ConnectionNotEstablishedException();
		}

		if (!string.IsNullOrWhiteSpace(migration.Command.Down))
		{
			await _repository.ExecuteAsync(migration.Command.Down, ct);
		}
	}

	private async Task<bool> IsDatabaseAvailableAsync()
	{
		return await _repository.TryHandshakeAsync();
	}
}
