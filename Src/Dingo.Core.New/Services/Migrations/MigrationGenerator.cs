using Dingo.Core.Exceptions;
using Dingo.Core.Extensions;
using Dingo.Core.Services.Adapters;
using Dingo.Core.Validators.Migration.Name;

namespace Dingo.Core.Services.Migrations;

internal class MigrationGenerator : IMigrationGenerator
{
	private readonly IMigrationNameValidator _migrationNameValidator;
	private readonly IDirectoryAdapter _directoryAdapter;
	private readonly IPathAdapter _pathAdapter;
	private readonly IFileAdapter _fileAdapter;

	public MigrationGenerator(
		IMigrationNameValidator migrationNameValidator,
		IDirectoryAdapter directoryAdapter,
		IPathAdapter pathAdapter,
		IFileAdapter fileAdapter
	)
	{
		_migrationNameValidator = migrationNameValidator.Required(nameof(migrationNameValidator));
		_directoryAdapter = directoryAdapter.Required(nameof(directoryAdapter));
		_pathAdapter = pathAdapter.Required(nameof(pathAdapter));
		_fileAdapter = fileAdapter.Required(nameof(fileAdapter));
	}

	public async Task GenerateAsync(string name, string path, CancellationToken ct = default)
	{
		if (!_migrationNameValidator.Validate(name))
		{
			throw new InvalidMigrationNameException();
		}

		if (!_directoryAdapter.Exists(path))
		{
			_directoryAdapter.CreateDirectory(path);
		}

		var filepath = BuildMigrationFileName(path, name);

		// TODO: Extract into EmptyMigrationTemplate
		_fileAdapter.Create(filepath).Close();
		await Task.CompletedTask;
	}

	private string BuildMigrationFileName(string path, string name)
	{
		return _pathAdapter.Join(
			_pathAdapter.GetAbsolutePath(path),
			$"{DateTime.UtcNow:yyyyMMddHHmmss}_{name}.sql"
		);
	}
}
