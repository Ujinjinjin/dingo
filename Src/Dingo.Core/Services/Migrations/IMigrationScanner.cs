using Dingo.Core.Models;

namespace Dingo.Core.Services.Migrations;

public interface IMigrationScanner
{
	Task<IReadOnlyList<Migration>> ScanAsync(string path, CancellationToken ct = default);
}
