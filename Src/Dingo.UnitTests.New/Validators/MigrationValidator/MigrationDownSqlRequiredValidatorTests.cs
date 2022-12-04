using Dingo.Core.New;
using Dingo.Core.New.Factories;
using Dingo.Core.New.Validators;
using Dingo.Core.New.Validators.MigrationValidator;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.UnitTests.New.Validators.MigrationValidator;

public class MigrationDownSqlRequiredValidatorTests
{
	private readonly IValidator<Migration> _validator;
	private readonly IMigrationFactory _migrationFactory;

	public MigrationDownSqlRequiredValidatorTests()
	{
		var iocModule = new UnitTestIocModule().Build();

		_validator = iocModule.GetRequiredService<MigrationDownSqlRequiredValidator>();
		_migrationFactory = iocModule.GetRequiredService<IMigrationFactory>();
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void MigrationDownSqlRequiredValidatorTests_Validate__WhenEmptyOrNullDownSqlGiven_ThenValidationFailed(string down)
	{
		// arrange
		var up = Guid.NewGuid().ToString();
		var migration = _migrationFactory.Create(up, down);

		// act
		var result = _validator.Validate(migration);

		// assert
		result.Should().BeFalse();
	}

	[Fact]
	public void MigrationDownSqlRequiredValidatorTests_Validate__WhenNonEmptyDownSqlGiven_ThenValidationPassed()
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
