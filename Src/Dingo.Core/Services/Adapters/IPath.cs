namespace Dingo.Core.Services.Adapters;

/// <summary> Path adapter </summary>
internal interface IPath
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

	/// <summary> Get directory path where application data files are stored </summary>
	string GetAppDataPath();

	/// <summary> Get directory path where logs are stored </summary>
	string GetLogsPath();

	/// <inheritdoc cref="Path.Join(string?[])"/>
	public string Join(params string?[] paths);
}
