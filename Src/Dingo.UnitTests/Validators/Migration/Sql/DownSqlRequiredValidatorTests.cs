using Dingo.Core;
using Dingo.Core.Models;
using Dingo.Core.Validators;
using Dingo.Core.Validators.Migration.Sql;
using Dingo.Core.Validators.Primitive;
using Trico.Configuration;

namespace Dingo.UnitTests.Validators.Migration.Sql;

public class DownSqlRequiredValidatorTests : UnitTestBase
{
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void MigrationDownSqlRequiredValidatorTests_Validate__WhenEmptyOrNullDownSqlGiven_ThenValidationFailed(string down)
	{
		// arrange
		var config = SetupConfiguration();
		var validator = CreateValidator(config);
		var up = Fixture.Create<string>();
		var command = new MigrationCommand(up, down);

		// act
		var result = validator.Validate(command);

		// assert
		result.Should().BeFalse();
	}

	[Fact]
	public void MigrationDownSqlRequiredValidatorTests_Validate__WhenNonEmptyDownSqlGiven_ThenValidationPassed()
	{
		// arrange
		var config = SetupConfiguration();
		var validator = CreateValidator(config);
		var up = Fixture.Create<string>();
		var down = Fixture.Create<string>();
		var command = new MigrationCommand(up, down);

		// act
		var result = validator.Validate(command);

		// assert
		result.Should().BeTrue();
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void DownSqlRequiredValidatorTests_Validate__GivenDownMigrationCommandNotRequired_WhenDownCommandNotGiven_ThenValidationPassed(string down)
	{
		// arrange
		var config = SetupConfiguration(false);
		var validator = CreateValidator(config);
		var up = Fixture.Create<string>();
		var command = new MigrationCommand(up, down);

		// act
		var result = validator.Validate(command);

		// assert
		result.Should().BeTrue();
	}

	private IValidator<MigrationCommand> CreateValidator(IConfiguration configuration)
	{
		var stringRequiredValidator = new StringRequiredValidator();
		return new DownSqlRequiredValidator(stringRequiredValidator, configuration);
	}

	private IConfiguration SetupConfiguration(bool downCommandRequired = true)
	{
		var value = downCommandRequired
			? "true"
			: "false";

		var config = new Mock<IConfiguration>();
		config.Setup(c => c.Get(It.Is<string>(name => name == Configuration.Key.MigrationDownRequired)))
			.Returns(value);

		return config.Object;
	}
}
