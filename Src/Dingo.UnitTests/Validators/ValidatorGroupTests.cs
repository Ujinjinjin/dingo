using Dingo.Core.Validators;

namespace Dingo.UnitTests.Validators;

public class ValidatorGroupTests : UnitTestBase
{
	[Fact]
	public void ValidatorGroupTests_Validate__WhenAnyOfGroupMembersFailValidation_ThenValidationFailed()
	{
		// arrange
		var validatorGroup = CreateValidatorGroupFailing();

		// act
		var result = validatorGroup.Validate(Fixture.Create<int>());

		// assert
		result.Should().BeFalse();
	}

	[Fact]
	public void ValidatorGroupTests_Validate__WhenAllGroupMembersPassValidation_ThenValidationPassed()
	{
		// arrange
		var validatorGroup = CreateValidatorGroupPassing();

		// act
		var result = validatorGroup.Validate(Fixture.Create<int>());

		// assert
		result.Should().BeTrue();
	}

	private ValidatorGroupHead<int, DummyValidator> CreateValidatorGroupFailing()
	{
		return CreateValidatorGroup(1);
	}

	private ValidatorGroupHead<int, DummyValidator> CreateValidatorGroupPassing()
	{
		return CreateValidatorGroup(0);
	}

	private ValidatorGroupHead<int, DummyValidator> CreateValidatorGroup(int failedCount)
	{
		var validationResults = CreateValidationResults(failedCount);
		var groupMembers = CreateValidatorGroupMembers(validationResults);
		return new DummyValidator(groupMembers);
	}

	private IEnumerable<bool> CreateValidationResults(int failedCount)
	{
		var rnd = new Random();
		var count = rnd.Next(Math.Min(failedCount, 10), Math.Max(failedCount, 100));
		var validationResults = new bool[count];

		for (var i = 0; i < count; i++)
		{
			validationResults[i] = i >= failedCount;
		}

		return validationResults;
	}

	private IEnumerable<IValidatorGroupMember<int, DummyValidator>> CreateValidatorGroupMembers(
		IEnumerable<bool> validationResults
	)
	{
		var groupMembers = new List<IValidatorGroupMember<int, DummyValidator>>();

		foreach (var validationResult in validationResults)
		{
			var mock = new Mock<IValidatorGroupMember<int, DummyValidator>>();

			mock.Setup(x => x.Validate(It.IsAny<int>()))
				.Returns(validationResult);

			groupMembers.Add(mock.Object);
		}

		return groupMembers;
	}

	internal sealed class DummyValidator : ValidatorGroupHead<int, DummyValidator>
	{
		public DummyValidator(
			IEnumerable<IValidatorGroupMember<int, DummyValidator>> groupMembers
		) : base(
			groupMembers
		)
		{
		}
	}
}
