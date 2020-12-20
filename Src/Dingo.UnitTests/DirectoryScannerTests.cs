using AutoFixture;
using Dingo.Core.Adapters;
using Dingo.Core.Helpers;
using Moq;
using System.IO;
using Xunit;

namespace Dingo.UnitTests
{
	public class DirectoryScannerTests : UnitTestsBase
	{
		[Fact]
		public void DirectoryScannerTests__GetFilePathList__WhenPathsGiven_ThenResultOrderedByModuleThenByFilename()
		{
			// Arrange
			var pathHelper = new PathHelper();
			var mockPathHelper = new Mock<IPathHelper>();
			var directoryAdapter = new Mock<IDirectoryAdapter>();

			var fixture = CreateFixture(directoryAdapter, mockPathHelper);
			var rootPath = "/usr/home/projects/dingo/";

			mockPathHelper
				.Setup(x => x.GetRootDirectory(It.IsAny<string>()))
				.Returns<string>(x => pathHelper.GetRootDirectory(x));
			directoryAdapter
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

			var directoryScanner = fixture.Create<DirectoryScanner>();

			// Act
			var filePathList = directoryScanner.GetFilePathList(rootPath, It.IsAny<string>());

			// Assert
			Assert.Equal("20201111000000_create.sql", filePathList[0].Filename);
			Assert.Equal("1. migrations", filePathList[0].Module);

			Assert.Equal("20201116000000_create.sql", filePathList[1].Filename);
			Assert.Equal("1. migrations", filePathList[1].Module);

			Assert.Equal("20201118000000_create.sql", filePathList[2].Filename);
			Assert.Equal("1. migrations", filePathList[2].Module);

			Assert.Equal("20201119000000_alter.sql", filePathList[3].Filename);
			Assert.Equal("1. migrations", filePathList[3].Module);

			Assert.Equal("t_user_filter.sql", filePathList[4].Filename);
			Assert.Equal("2. date_types", filePathList[4].Module);

			Assert.Equal("create_user.sql", filePathList[5].Filename);
			Assert.Equal("3. procedures", filePathList[5].Module);

			Assert.Equal("get_user_by_id.sql", filePathList[6].Filename);
			Assert.Equal("3. procedures", filePathList[6].Module);
		}
	}
}
