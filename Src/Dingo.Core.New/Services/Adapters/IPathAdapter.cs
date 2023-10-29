namespace Dingo.Core.Services.Adapters;

/// <summary> Path adapter </summary>
internal interface IPathAdapter
{
	/// <summary> Clean path and unify all slashes </summary>
	string CleanPath(string path);

	/// <inheritdoc cref="Path.GetFileName(string?)"/>
	string GetFileName(string path);

	/// <inheritdoc cref="Path.GetRelativePath(string, string)"/>
	string GetRelativePath(string relativeTo, string path);

	/// <summary> Get absolute path from relative. Takes current execution directory as root </summary>
	string GetAbsolutePath(string relativePath);

	/// <summary> Extract root directory name from path </summary>
	string GetRootDirectory(string path);

	/// <summary> Get application installation directory path </summary>
	string GetApplicationPath();

	/// <inheritdoc cref="Path.Join(string?[])"/>
	public string Join(params string?[] paths);
}
