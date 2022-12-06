using Dingo.Core;
using Dingo.Core.Factories;
using Dingo.Core.Validators;
using Dingo.Core.Validators.MigrationValidators.SqlValidators;
using Dingo.Core.Validators.Primitive;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.UnitTests.Validators.MigrationValidators.SqlValidators;

public class MigrationUpSqlRequiredValidatorTests : UnitTestBase
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
		var migration = CreateMigration(up, down);

		// act
		var result = validator.Validate(migration);

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
		var migration = CreateMigration(up, down);

		// act
		var result = validator.Validate(migration);

		// assert
		result.Should().BeTrue();
	}

	private IValidator<Migration> CreateValidator()
	{
		var stringRequiredValidator = new StringRequiredValidator();
		return new MigrationUpSqlRequiredValidator(stringRequiredValidator);
	}
}
