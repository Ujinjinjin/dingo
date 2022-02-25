using Dingo.Core.Models;

namespace Dingo.Core.Extensions;

/// <summary> Collection of extensions for <see cref="MigrationStatus"/> </summary>
public static class MigrationStatusExtensions
{
	/// <summary> Converts <see cref="MigrationStatus"/> to display text </summary>
	/// <param name="source">Migration status</param>
	/// <returns>Display text</returns>
	/// <exception cref="ArgumentOutOfRangeException">Unknown migration status</exception>
	public static string ToDisplayText(this MigrationStatus source)
	{
		return source switch
		{
			MigrationStatus.Unknown => "Unknown",
			MigrationStatus.New => "New",
			MigrationStatus.Outdated => "Outdated",
			MigrationStatus.UpToDate => "Up to date",
			_ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
		};
	}
}