﻿namespace Dingo.Core.Helpers
{
	/// <summary> Path helper </summary>
	internal interface IPathHelper
	{
		/// <summary> Build file path by directory, filename and file extension </summary>
		/// <param name="path">Directory where file is placed</param>
		/// <param name="filename">Filename</param>
		/// <param name="extension">File extension, e.g. `.yml`</param>
		/// <returns>Absolute path to the file</returns>
		string BuildFilePath(string path, string filename, string extension);
		
		/// <summary> Get directory where application executables are stored </summary>
		/// <returns>Application base directory</returns>
		string GetApplicationBaseDirectory();
		
		/// <summary> Get directory from where application was executed </summary>
		/// <returns>Execution base directory</returns>
		string GetExecutionBaseDirectory();
		
		/// <summary> Build absolute path from relative to execution directory </summary>
		/// <param name="relativePath">Relative path</param>
		/// <returns>Absolute path</returns>
		string GetAbsolutePathFromRelative(string relativePath);
		
		/// <summary> Build absolute path from relative to application base directory </summary>
		/// <param name="relativePath">Relative path</param>
		/// <returns>Absolute path</returns>
		string GetAppRootPathFromRelative(string relativePath);
	}
}
