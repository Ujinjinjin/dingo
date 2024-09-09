using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Dingo.Core.Services.Adapters;
using Dingo.Core.Services.Helpers;
using Trico.Configuration;

namespace Dingo.Core.Services.Migrations;

internal sealed class MigrationScanner : IMigrationScanner
{
	private readonly IConfiguration _configuration;
	private readonly IMigrationCommandParser _commandParser;
	private readonly IDirectoryScanner _directoryScanner;
	private readonly IFile _file;

	public MigrationScanner(
		IConfiguration configuration,
		IMigrationCommandParser commandParser,
		IDirectoryScanner directoryScanner,
		IFile file
	)
	{
		_configuration = configuration.Required(nameof(configuration));
		_commandParser = commandParser.Required(nameof(commandParser));
		_directoryScanner = directoryScanner.Required(nameof(directoryScanner));
		_file = file.Required(nameof(file));
	}

	public async Task<IReadOnlyList<Migration>> ScanAsync(
		string path,
		CancellationToken ct = default
	)
	{
		var migrations = new List<Migration>();

		var migrationFiles = _directoryScanner.Scan(
			path,
			_configuration.Get(Configuration.Key.MigrationWildcard)
		);

		foreach (var migrationPath in migrationFiles)
		{
			var migrationContent = await _file.ReadAllTextAsync(migrationPath.Absolute, ct);
			var command = _commandParser.Parse(migrationContent);
			var hash = await Hash.ComputeAsync(_file, migrationPath);

			var migration = new Migration(migrationPath, hash, command);
			migrations.Add(migration);
		}

		return migrations;
	}
}
