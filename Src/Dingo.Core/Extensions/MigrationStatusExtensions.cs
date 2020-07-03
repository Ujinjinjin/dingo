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
			switch (source)
			{
				case MigrationStatus.Unknown:
					return "Unknown migration status.";
				case MigrationStatus.New:
					return "Migration is new and will be installed.";
				case MigrationStatus.Outdated:
					return "Migration is outdated, newer version will be installed.";
				case MigrationStatus.UpToDate:
					return "Migration is up to date. No actions required.";
				default:
					throw new ArgumentOutOfRangeException(nameof(source), source, null);
			}
		}
	}
}
