using Dingo.Core.Adapters;
using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Dingo.Core.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dingo.Core.Helpers;

/// <inheritdoc />
internal sealed class DirectoryScanner : IDirectoryScanner
{
	private readonly IDirectoryAdapter _directoryAdapter;
	private readonly IPathHelper _pathHelper;
	private readonly IValidator<string> _migrationNameValidator;

	public DirectoryScanner(
		IDirectoryAdapter directoryAdapter,
		IPathHelper pathHelper,
		MigrationNameValidator migrationNameValidator
	)
	{
		_directoryAdapter = directoryAdapter ?? throw new ArgumentNullException(nameof(directoryAdapter));
		_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
		_migrationNameValidator = migrationNameValidator ?? throw new ArgumentNullException(nameof(migrationNameValidator));
	}

	/// <inheritdoc />
	public IList<FilePath> GetFilePathList(string rootPath, string searchPattern)
	{
		var fileList = _directoryAdapter.GetFiles(rootPath, searchPattern, SearchOption.AllDirectories);

		var filePathList = new FilePath[fileList.Length];

		for (var i = 0; i < fileList.Length; i++)
		{
			var absolutePath = fileList[i].ReplaceBackslashesWithSlashes();
			var relativePath = absolutePath.Replace(rootPath, string.Empty);
			var filename = Path.GetFileName(absolutePath);
			filePathList[i] = new FilePath
			{
				Absolute = absolutePath,
				Relative = relativePath,
				Filename = filename,
				Module = _pathHelper.GetRootDirectory(relativePath),
				IsValid = _migrationNameValidator.Validate(filename),
			};
		}

		return filePathList
			.OrderBy(x => x.Module)
			.ThenBy(x => x.Filename)
			.ToArray();
	}
}