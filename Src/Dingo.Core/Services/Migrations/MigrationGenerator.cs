using Dingo.Core.Exceptions;
using Dingo.Core.Extensions;
using Dingo.Core.Services.Adapters;
using Dingo.Core.Validators.Migration.Name;

namespace Dingo.Core.Services.Migrations;

internal class MigrationGenerator : IMigrationGenerator
{
	private readonly IMigrationNameValidator _migrationNameValidator;
	private readonly IDirectory _directory;
	private readonly IPath _path;
	private readonly IFile _file;

	public MigrationGenerator(
		IMigrationNameValidator migrationNameValidator,
		IDirectory directory,
		IPath path,
		IFile file
	)
	{
		_migrationNameValidator = migrationNameValidator.Required(nameof(migrationNameValidator));
		_directory = directory.Required(nameof(directory));
		_path = path.Required(nameof(path));
		_file = file.Required(nameof(file));
	}

	public async Task GenerateAsync(string name, string path, CancellationToken ct = default)
	{
		if (!_migrationNameValidator.Validate(name))
		{
			throw new InvalidMigrationNameException();
		}

		if (!_directory.Exists(path))
		{
			_directory.CreateDirectory(path);
		}

		var filepath = BuildMigrationFileName(path, name);

		// TODO: Extract into EmptyMigrationTemplate
		_file.Create(filepath).Close();
		await Task.CompletedTask;
	}

	private string BuildMigrationFileName(string path, string name)
	{
		return _path.Join(
			_path.GetAbsolutePath(path),
			$"{DateTime.UtcNow:yyyyMMddHHmmss}_{name}.sql"
		);
	}
}
