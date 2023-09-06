using System.Runtime.CompilerServices;
using Dingo.Core.Models;

namespace Dingo.Core.Migrations;

public interface IMigrationScanner
{
	Task<IReadOnlyCollection<Migration>> ScanAsync(string path, CancellationToken cancellationToken = default);
}