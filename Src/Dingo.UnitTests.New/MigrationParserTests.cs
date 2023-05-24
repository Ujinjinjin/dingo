using Dingo.Core;
using Dingo.Core.Exceptions;
using System.Text;
using Trico.Configuration;

namespace Dingo.UnitTests;

public sealed class MigrationParserTests : UnitTestBase
{
	private const string Delimiter = "-- down";

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData(null)]
	public void MigrationParserTests_Parse__WhenEmptySqlGiven_ThenEmptyMigrationReturned(string sql)
	{
		// arrange
		var config = SetupConfiguration(Delimiter);
		var migrationParser = new MigrationParser(config);

		// act
		var migration = migrationParser.Parse(sql);

		// assert
		migration.Should().Be(Migration.Empty);
	}

	[Fact]
	public void MigrationParserTests_Parse__WhenSqlWithNoDownCommandGiven_ThenMigrationNotContainsDownCommand()
	{
		// arrange
		var config = SetupConfiguration(Delimiter);
		var migrationParser = new MigrationParser(config);
		var sql = Fixture.Create<string>();

		// act
		var migration = migrationParser.Parse(sql);

		// assert
		migration.Should().NotBe(Migration.Empty);
		migration.Up.Should().NotBeNull();
		migration.Up.Should().Be(sql);
		migration.Down.Should().BeNull();
	}

	[Fact]
	public void MigrationParserTests_Parse__WhenSqlWithEmptyDownCommandGiven_ThenMigrationNotContainsDownCommand()
	{
		// arrange
		var config = SetupConfiguration(Delimiter);
		var migrationParser = new MigrationParser(config);
		var sb = new StringBuilder();
		var up = Fixture.Create<string>();
		var sql = sb
			.Append(up)
			.Append(Environment.NewLine)
			.Append(Delimiter)
			.ToString();

		// act
		var migration = migrationParser.Parse(sql);

		// assert
		migration.Should().NotBe(Migration.Empty);
		migration.Up.Should().NotBeNull();
		migration.Up.Should().Be(up);
		migration.Down.Should().BeNull();
	}

	[Fact]
	public void MigrationParserTests_Parse__WhenSqlWithDownCommandGiven_ThenMigrationContainsBothCommands()
	{
		// arrange
		var config = SetupConfiguration(Delimiter);
		var migrationParser = new MigrationParser(config);
		var sb = new StringBuilder();
		var up = Fixture.Create<string>();
		var down = Fixture.Create<string>();
		var sql = sb
			.Append(up)
			.Append(Environment.NewLine)
			.Append(Delimiter)
			.Append(Environment.NewLine)
			.Append(down)
			.ToString();

		// act
		var migration = migrationParser.Parse(sql);

		// assert
		migration.Should().NotBe(Migration.Empty);
		migration.Up.Should().NotBeNull();
		migration.Up.Should().Be(up);
		migration.Down.Should().NotBeNull();
		migration.Down.Should().Be(down);
	}

	[Fact]
	public void MigrationParserTests_Parse__WhenCommandWithMultipleDelimitersGiven_ThenErrorThrown()
	{
		// arrange
		var config = SetupConfiguration(Delimiter);
		var migrationParser = new MigrationParser(config);
		var sb = new StringBuilder();
		var up = Fixture.Create<string>();
		var down = Fixture.Create<string>();
		var sql = sb
			.Append(up)
			.Append(Environment.NewLine)
			.Append(Delimiter)
			.Append(Environment.NewLine)
			.Append(down)
			.Append(Delimiter)
			.ToString();

		// act
		var action = () => migrationParser.Parse(sql);

		// assert
		action.Should().Throw<MigrationParsingException>();
	}

	private IConfiguration SetupConfiguration(string delimiter)
	{
		var config = new Mock<IConfiguration>();
		config.Setup(c => c.Get(It.IsAny<string>())).Returns(delimiter);

		return config.Object;
	}
}
