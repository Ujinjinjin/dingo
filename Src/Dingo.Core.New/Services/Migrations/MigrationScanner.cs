using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Dingo.Core.Services.Adapters;
using Dingo.Core.Services.Helpers;
using Trico.Configuration;

namespace Dingo.Core.Services.Migrations;

internal class MigrationScanner : IMigrationScanner
{
	private readonly IConfiguration _configuration;
	private readonly IMigrationCommandParser _commandParser;
	private readonly IDirectoryScanner _directoryScanner;
	private readonly IFileAdapter _fileAdapter;

	public MigrationScanner(
		IConfiguration configuration,
		IMigrationCommandParser commandParser,
		IDirectoryScanner directoryScanner,
		IFileAdapter fileAdapter
	)
	{
		_configuration = configuration.Required(nameof(configuration));
		_commandParser = commandParser.Required(nameof(commandParser));
		_directoryScanner = directoryScanner.Required(nameof(directoryScanner));
		_fileAdapter = fileAdapter.Required(nameof(fileAdapter));
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
			var migrationContent = await _fileAdapter.ReadAllTextAsync(migrationPath.Absolute, ct);
			var command = _commandParser.Parse(migrationContent);
			var hash = await Hash.ComputeAsync(_fileAdapter, migrationPath);

			var migration = new Migration(migrationPath, hash, command);
			migrations.Add(migration);
		}

		return migrations;
	}
}