using Dingo.Core.Models;
using System;

namespace Dingo.Core.Extensions
{
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
				MigrationStatus.Unknown => "Unknown migration status.",
				MigrationStatus.New => "Migration is new and will be installed.",
				MigrationStatus.Outdated => "Migration is outdated, newer version will be installed.",
				MigrationStatus.UpToDate => "Migration is up to date. No actions required.",
				_ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
			};
		}
	}
}
