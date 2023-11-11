using Dingo.Core.Services.Adapters;
using Dingo.Core.Services.Helpers;

namespace Dingo.UnitTests;

public class DirectoryScannerTests : UnitTestBase
{
	[Fact]
	public void DirectoryScannerTests__GetFilePathList__WhenGivenPathContainsFiles_ThenResultOrderedByModuleThenByFilename()
	{
		// arrange
		var rootPath = "/usr/home/projects/dingo/";
		var pathAdapter = SetupPathAdapter();
		var directoryAdapter = SetupDirectoryAdapter(rootPath);
		var directoryScanner = new DirectoryScanner(directoryAdapter, pathAdapter);

		// act
		var filePathList = directoryScanner.Scan(rootPath, It.IsAny<string>());
		var filenames = filePathList.Select(x => $"{x.Module}/{x.Filename}").ToArray();

		// assert
		filenames.Should().BeInAscendingOrder();
	}

	private IPath SetupPathAdapter()
	{
		var pathHelper = new PathAdapter();
		var adapter = new Mock<IPath>();
		adapter
			.Setup(x => x.GetRootDirectory(It.IsAny<string>()))
			.Returns<string>(x => pathHelper.GetRootDirectory(x));

		return adapter.Object;
	}

	private IDirectory SetupDirectoryAdapter(string rootPath)
	{
		var adapter = new Mock<IDirectory>();

		adapter
			.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
			.Returns(new[]
			{
				$"{rootPath}1. migrations/dingo_migrations/20201116000000_create.sql",
				$"{rootPath}1. migrations/dingo_migrations/20201119000000_alter.sql",
				$"{rootPath}1. migrations/users/20201118000000_create.sql",
				$"{rootPath}1. migrations/work_item/20201111000000_create.sql",
				$"{rootPath}2. date_types/t_user_filter.sql",
				$"{rootPath}3. procedures/create_user.sql",
				$"{rootPath}3. procedures/get_user_by_id.sql",
			});

		return adapter.Object;
	}
}
