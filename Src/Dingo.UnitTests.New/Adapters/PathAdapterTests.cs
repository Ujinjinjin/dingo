using Dingo.Core.Adapters;

namespace Dingo.UnitTests.Adapters;

public class PathAdapterTests : UnitTestBase
{
	[Fact]
	public void PathAdapterTests_GetRootDirectory__WhenPathWithSingleDirectoryGiven_ThenRootPathIsTheGivenDirectory()
	{
		// arrange
		var adapter = new PathAdapter();
		var path = Fixture.Create<string>();

		// act
		var rootPath = adapter.GetRootDirectory(path);

		// assert
		rootPath.Should().Be(path);
	}

	[Theory]
	[InlineData("root/src", "root")]
	[InlineData("root/src/module-1", "root")]
	[InlineData("root/src/module-2", "root")]
	[InlineData("root/src/module-2/sub-module", "root")]
	public void PathAdapterTests_GetRootDirectory__WhenPathWithMultipleLevelsGiven_ThenRootPathIsEqualToFirstDirectory(
		string path,
		string expected
	)
	{
		// arrange
		var adapter = new PathAdapter();

		// act
		var rootPath = adapter.GetRootDirectory(path);

		// assert
		rootPath.Should().Be(expected);
	}

	[Theory]
	[InlineData("", "")]
	[InlineData("root\\src", "root/src")]
	[InlineData("root/src", "root/src")]
	[InlineData("root/src\\module-1", "root/src/module-1")]
	[InlineData("root\\src/module-1", "root/src/module-1")]
	public void PathAdapterTests_CleanPath__WhenPathWithDifferentSlashesGiven_ThenAllSlashesUnified(string path,
		string expected)
	{
		// arrange
		var adapter = new PathAdapter();

		// act
		var cleanPath = adapter.CleanPath(path);

		// assert
		cleanPath.Should().Be(expected);
	}

	[Theory]
	[InlineData("root/scr/module-1/dingo.sql", "dingo.sql")]
	[InlineData("root/scr/module-2/create_table.sql", "create_table.sql")]
	[InlineData("root/scr/config.json", "config.json")]
	[InlineData("config.json", "config.json")]
	[InlineData("", "")]
	[InlineData(null, null)]
	public void PathAdapterTests_GetFileName__WhenPathToFileGiven_ThenOnlyFilenameReturned(string path, string expected)
	{
		// arrange
		var adapter = new PathAdapter();

		// act
		var fileName = adapter.GetFileName(path);

		// assert
		fileName.Should().Be(expected);
	}

	[Theory]
	[InlineData("/usr/home/dingo/scr/dingo.sql", "/usr/home/dingo/", "scr/dingo.sql")]
	[InlineData("scr/dingo.sql", "./", "scr/dingo.sql")]
	[InlineData("scr/dingo.sql", ".", "scr/dingo.sql")]
	public void PathAdapterTests_GetRelativePath__WhenAbsolutePathAndBaseDirGiven_ThenRelativePathReturned(
		string absolutePath,
		string root,
		string expected
	)
	{
		// arrange
		var adapter = new PathAdapter();

		// act
		var relativePath = adapter.GetRelativePath(root, absolutePath);

		// assert
		relativePath.Should().Be(expected);
	}
}
