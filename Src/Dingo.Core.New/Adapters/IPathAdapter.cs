namespace Dingo.Core.Adapters;

/// <summary> Path adapter </summary>
internal interface IPathAdapter
{
	/// <summary> Clean path and unify all slashes </summary>
	string CleanPath(string path);

	/// <inheritdoc cref="Path.GetFileName(string?)"/>
	string GetFileName(string path);

	/// <inheritdoc cref="Path.GetRelativePath(string, string)"/>
	string GetRelativePath(string relativeTo, string path);

	/// <summary> Extract root directory name from path </summary>
	string GetRootDirectory(string path);
}
