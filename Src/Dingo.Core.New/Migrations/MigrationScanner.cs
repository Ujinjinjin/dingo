using Dingo.Core.Adapters;
using Dingo.Core.Helpers;
using Dingo.Core.Models;
using Trico.Configuration;

namespace Dingo.Core.Migrations;

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
		_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		_commandParser = commandParser ?? throw new ArgumentNullException(nameof(commandParser));
		_directoryScanner = directoryScanner ?? throw new ArgumentNullException(nameof(directoryScanner));
		_fileAdapter = fileAdapter ?? throw new ArgumentNullException(nameof(fileAdapter));
	}

	public async Task<IReadOnlyCollection<Migration>> ScanAsync(
		string path,
		CancellationToken cancellationToken = default
	)
	{
		var migrations = new List<Migration>();

		var migrationFiles = _directoryScanner.Scan(
			path,
			_configuration.Get(Configuration.Key.MigrationWildcard)
		);

		foreach (var migrationPath in migrationFiles)
		{
			var migrationContent = await _fileAdapter.ReadAllTextAsync(migrationPath.Absolute, cancellationToken);
			var command = _commandParser.Parse(migrationContent);
			var hash = await Hash.ComputeAsync(_fileAdapter, migrationPath);

			var migration = new Migration(migrationPath, hash, command);
			migrations.Add(migration);
		}

		return migrations;
	}
}
