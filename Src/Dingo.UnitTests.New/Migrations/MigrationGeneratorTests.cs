using Dingo.Core.Adapters;
using Dingo.Core.Exceptions;
using Dingo.Core.Migrations;
using Dingo.Core.Validators.Migration.Name;

namespace Dingo.UnitTests.Migrations;

public class MigrationGeneratorTests : UnitTestBase
{
	[Fact]
	public async Task MigrationGeneratorTests_CreateAsync__WhenInvalidNameIsGiven_ThenExceptionIsThrown()
	{
		// arrange
		var validator = SetupValidator(false);
		var directory = SetupDirectoryAdapter();
		var path = SetupPathAdapter();
		var file = SetupFileAdapter();
		var generator = new MigrationGenerator(validator, directory, path, file);


		var func = () => generator.GenerateAsync(Fixture.Create<string>(), Fixture.Create<string>());

		// assert
		await func.Should().ThrowAsync<InvalidMigrationNameException>();
	}

	private IMigrationNameValidator SetupValidator(bool validationResult = true)
	{
		var validator = new Mock<IMigrationNameValidator>();

		validator.Setup(x => x.Validate(It.IsAny<string>()))
			.Returns(validationResult);

		return validator.Object;
	}

	private IDirectoryAdapter SetupDirectoryAdapter()
	{
		var adapter = new Mock<IDirectoryAdapter>();
		return adapter.Object;
	}

	private IPathAdapter SetupPathAdapter()
	{
		var adapter = new Mock<IPathAdapter>();
		return adapter.Object;
	}

	private IFileAdapter SetupFileAdapter()
	{
		var adapter = new Mock<IFileAdapter>();
		return adapter.Object;
	}
}
