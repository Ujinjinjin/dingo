using Dingo.Core;
using Dingo.Core.Validators;
using Dingo.Core.Validators.MigrationValidators.SqlValidators;
using Dingo.Core.Validators.Primitive;

namespace Dingo.UnitTests.New.Validators.MigrationValidators.SqlValidators;

public class MigrationDownSqlRequiredValidatorTests : UnitTestBase
{
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void MigrationDownSqlRequiredValidatorTests_Validate__WhenEmptyOrNullDownSqlGiven_ThenValidationFailed(string down)
	{
		// arrange
		var validator = CreateValidator();
		var up = Fixture.Create<string>();
		var migration = CreateMigration(up, down);

		// act
		var result = validator.Validate(migration);

		// assert
		result.Should().BeFalse();
	}

	[Fact]
	public void MigrationDownSqlRequiredValidatorTests_Validate__WhenNonEmptyDownSqlGiven_ThenValidationPassed()
	{
		// arrange
		var validator = CreateValidator();
		var up = Fixture.Create<string>();
		var down = Fixture.Create<string>();
		var migration = CreateMigration(up, down);

		// act
		var result = validator.Validate(migration);

		// assert
		result.Should().BeTrue();
	}

	private IValidator<Migration> CreateValidator()
	{
		var stringRequiredValidator = new StringRequiredValidator();
		return new MigrationDownSqlRequiredValidator(stringRequiredValidator);
	}
}
