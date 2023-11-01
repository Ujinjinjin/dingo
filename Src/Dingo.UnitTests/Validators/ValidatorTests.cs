using Dingo.Core.Validators;

namespace Dingo.UnitTests.Validators;

public class ValidatorTests : UnitTestBase
{
	[Fact]
	public void ValidatorTests_Validate__WhenNoValidationRulesGiven_ThenValidationPassed()
	{
		// arrange
		var validator = new DummyValidator<int>();
		var value = Fixture.Create<int>();

		// act
		var result = validator.Validate(value);

		// assert
		result.Should().BeTrue();
	}

	private sealed class DummyValidator<T> : Validator<T>
	{
		protected override IReadOnlyList<Func<T, bool>> GetValidationRules()
		{
			return Array.Empty<Func<T, bool>>();
		}
	}
}
