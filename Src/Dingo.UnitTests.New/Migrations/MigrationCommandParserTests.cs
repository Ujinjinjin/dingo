using System.Text;
using Dingo.Core;
using Dingo.Core.Exceptions;
using Dingo.Core.Migrations;
using Dingo.Core.Models;
using Trico.Configuration;

namespace Dingo.UnitTests.Migrations;

public sealed class MigrationCommandParserTests : UnitTestBase
{
	private const string Delimiter = "-- down";

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData(null)]
	public void MigrationCommandParserTests_Parse__WhenEmptySqlGiven_ThenEmptyMigrationReturned(string sql)
	{
		// arrange
		var config = SetupConfiguration(Delimiter);
		var commandParser = new MigrationCommandParser(config);

		// act
		var command = commandParser.Parse(sql);

		// assert
		command.Should().Be(MigrationCommand.Empty);
	}

	[Fact]
	public void MigrationCommandParserTests_Parse__WhenSqlWithNoDownCommandGiven_ThenMigrationNotContainsDownCommand()
	{
		// arrange
		var config = SetupConfiguration(Delimiter);
		var commandParser = new MigrationCommandParser(config);
		var sql = Fixture.Create<string>();

		// act
		var command = commandParser.Parse(sql);

		// assert
		command.Should().NotBe(Migration.Empty);
		command.Up.Should().NotBeNull();
		command.Up.Should().Be(sql);
		command.Down.Should().BeNull();
	}

	[Fact]
	public void MigrationCommandParserTests_Parse__WhenSqlWithEmptyDownCommandGiven_ThenMigrationNotContainsDownCommand()
	{
		// arrange
		var config = SetupConfiguration(Delimiter);
		var commandParser = new MigrationCommandParser(config);
		var sb = new StringBuilder();
		var up = Fixture.Create<string>();
		var sql = sb
			.Append(up)
			.Append(Environment.NewLine)
			.Append(Delimiter)
			.ToString();

		// act
		var command = commandParser.Parse(sql);

		// assert
		command.Should().NotBe(Migration.Empty);
		command.Up.Should().NotBeNull();
		command.Up.Should().Be(up);
		command.Down.Should().BeNull();
	}

	[Fact]
	public void MigrationCommandParserTests_Parse__WhenSqlWithDownCommandGiven_ThenMigrationContainsBothCommands()
	{
		// arrange
		var config = SetupConfiguration(Delimiter);
		var commandParser = new MigrationCommandParser(config);
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
		var command = commandParser.Parse(sql);

		// assert
		command.Should().NotBe(Migration.Empty);
		command.Up.Should().NotBeNull();
		command.Up.Should().Be(up);
		command.Down.Should().NotBeNull();
		command.Down.Should().Be(down);
	}

	[Fact]
	public void MigrationCommandParserTests_Parse__WhenCommandWithMultipleDelimitersGiven_ThenExceptionThrown()
	{
		// arrange
		var config = SetupConfiguration(Delimiter);
		var commandParser = new MigrationCommandParser(config);
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
		var action = () => commandParser.Parse(sql);

		// assert
		action.Should().Throw<MigrationParsingException>();
	}

	private IConfiguration SetupConfiguration(string delimiter)
	{
		var config = new Mock<IConfiguration>();
		config.Setup(c => c.Get(It.Is<string>(name => name == Configuration.Key.MigrationDelimiter)))
			.Returns(delimiter);

		return config.Object;
	}
}
