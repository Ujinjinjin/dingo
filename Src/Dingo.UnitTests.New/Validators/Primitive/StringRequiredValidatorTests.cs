using Dingo.Core.New.Validators.Primitive;
using FluentAssertions;

namespace Dingo.UnitTests.New.Validators.Primitive;

public class StringRequiredValidatorTests
{
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void StringRequiredValidatorTests_Validate__WhenEmptyOrNullValueGiven_ThenValidationFailed(string value)
	{
		// arrange
		var validator = new StringRequiredValidator();

		// act
		var result = validator.Validate(value);

		// assert
		result.Should().BeFalse();
	}

	[Fact]
	public void StringRequiredValidatorTests_Validate__WhenNonEmptyValueGiven_ThenValidationPassed()
	{
		// arrange
		var validator = new StringRequiredValidator();
		var value = Guid.NewGuid().ToString();

		// act
		var result = validator.Validate(value);

		// assert
		result.Should().BeTrue();
	}
}
