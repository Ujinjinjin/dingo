using Dingo.Core.Validators;
using Dingo.Core.Validators.Migration.Name;
using Dingo.Core.Validators.Primitive;

namespace Dingo.UnitTests.Validators.Migration.Name;

public class MigrationNameValidatorTests : UnitTestBase
{
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void MigrationNameValidatorTests_Validate__WhenEmptyNameGiven_ThenValidationFailed(string name)
	{
		// arrange
		var validator = CreateValidator();

		// act
		var result = validator.Validate(name);

		// assert
		result.Should().BeFalse();
	}

	[Theory]
	[InlineData("create_table")]
	[InlineData("create_table__users")]
	[InlineData("drop_table__users2")]
	public void MigrationNameValidatorTests_Validate__WhenValidMigrationNameGiven_ThenValidationPassed(string name)
	{
		// arrange
		var validator = CreateValidator();

		// act
		var result = validator.Validate(name);

		// assert
		result.Should().BeTrue();
	}

	[Theory]
	[InlineData("create_table.sql")]
	[InlineData("create_table__users.txt")]
	[InlineData("drop_table__users2.mp4")]
	public void MigrationNameValidatorTests_Validate__WhenMigrationNameWithExtensionGiven_ThenValidationFailed(string name)
	{
		// arrange
		var validator = CreateValidator();

		// act
		var result = validator.Validate(name);

		// assert
		result.Should().BeFalse();
	}

	[Theory]
	[InlineData("create_table.")]
	[InlineData("create_table\\__users.txt")]
	[InlineData("drop_table__user/")]
	[InlineData("drop_table__()")]
	public void MigrationNameValidatorTests_Validate__WhenMigrationNameContainsSpecialCharacters_ThenValidationFailed(string name)
	{
		// arrange
		var validator = CreateValidator();

		// act
		var result = validator.Validate(name);

		// assert
		result.Should().BeFalse();
	}

	private IValidator<string?> CreateValidator()
	{
		var stringRequiredValidator = new StringRequiredValidator();
		return new MigrationNameValidator(stringRequiredValidator);
	}
}
