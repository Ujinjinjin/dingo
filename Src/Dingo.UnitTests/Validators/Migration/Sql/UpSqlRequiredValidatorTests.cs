using Dingo.Core.Models;
using Dingo.Core.Validators;
using Dingo.Core.Validators.Migration.Sql;
using Dingo.Core.Validators.Primitive;

namespace Dingo.UnitTests.Validators.Migration.Sql;

public class UpSqlRequiredValidatorTests : UnitTestBase
{
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void MigrationUpSqlRequiredValidatorTests_Validate__WhenEmptyOrNullUpSqlGiven_ThenValidationFailed(string up)
	{
		// arrange
		var validator = CreateValidator();
		var down = Fixture.Create<string>();
		var command = new MigrationCommand(up, down);

		// act
		var result = validator.Validate(command);

		// assert
		result.Should().BeFalse();
	}

	[Fact]
	public void MigrationUpSqlRequiredValidatorTests_Validate__WhenNonEmptyUpSqlGiven_ThenValidationPassed()
	{
		// arrange
		var validator = CreateValidator();
		var up = Fixture.Create<string>();
		var down = Fixture.Create<string>();
		var command = new MigrationCommand(up, down);

		// act
		var result = validator.Validate(command);

		// assert
		result.Should().BeTrue();
	}

	private IValidator<MigrationCommand> CreateValidator()
	{
		var stringRequiredValidator = new StringRequiredValidator();
		return new UpSqlRequiredValidator(stringRequiredValidator);
	}
}
