using Dingo.Core.Exceptions;
using Dingo.Core.Services.Adapters;
using Dingo.Core.Services.Migrations;
using Dingo.Core.Validators.Migration.Name;

namespace Dingo.UnitTests.Migrations;

public class MigrationGeneratorTests : UnitTestBase
{
	[Fact]
	public async Task MigrationGeneratorTests_CreateAsync__WhenInvalidNameIsGiven_ThenExceptionThrown()
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

	private IDirectory SetupDirectoryAdapter()
	{
		var adapter = new Mock<IDirectory>();
		return adapter.Object;
	}

	private IPath SetupPathAdapter()
	{
		var adapter = new Mock<IPath>();
		return adapter.Object;
	}

	private IFile SetupFileAdapter()
	{
		var adapter = new Mock<IFile>();
		return adapter.Object;
	}
}
