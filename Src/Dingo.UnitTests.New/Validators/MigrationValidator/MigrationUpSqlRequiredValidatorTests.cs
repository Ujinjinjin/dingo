using Dingo.Core.New;
using Dingo.Core.New.Factories;
using Dingo.Core.New.Validators;
using Dingo.Core.New.Validators.MigrationValidator;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.UnitTests.New.Validators.MigrationValidator;

public class MigrationUpSqlRequiredValidatorTests
{
	private readonly IValidator<Migration> _validator;
	private readonly IMigrationFactory _migrationFactory;

	public MigrationUpSqlRequiredValidatorTests()
	{
		var iocModule = new UnitTestIocModule().Build();

		_validator = iocModule.GetRequiredService<MigrationUpSqlRequiredValidator>();
		_migrationFactory = iocModule.GetRequiredService<IMigrationFactory>();
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void MigrationUpSqlRequiredValidatorTests_Validate__WhenEmptyOrNullUpSqlGiven_ThenValidationFailed(string up)
	{
		// arrange
		var down = Guid.NewGuid().ToString();
		var migration = _migrationFactory.Create(up, down);

		// act
		var result = _validator.Validate(migration);

		// assert
		result.Should().BeFalse();
	}

	[Fact]
	public void MigrationUpSqlRequiredValidatorTests_Validate__WhenNonEmptyUpSqlGiven_ThenValidationPassed()
	{
		// arrange
		var up = Guid.NewGuid().ToString();
		var down = Guid.NewGuid().ToString();
		var migration = _migrationFactory.Create(up, down);

		// act
		var result = _validator.Validate(migration);

		// assert
		result.Should().BeTrue();
	}
}
