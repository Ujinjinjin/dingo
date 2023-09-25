using System.Runtime.CompilerServices;
using Dingo.Core.Models;

namespace Dingo.Core.Migrations;

public interface IMigrationScanner
{
	Task<IReadOnlyList<Migration>> ScanAsync(string path, CancellationToken ct = default);
}
