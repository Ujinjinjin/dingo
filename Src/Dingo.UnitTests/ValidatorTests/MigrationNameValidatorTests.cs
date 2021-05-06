using Dingo.Core.Validators;
using Xunit;

namespace Dingo.UnitTests.ValidatorTests
{
	public class MigrationNameValidatorTests : UnitTestsBase
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("migration")]
		[InlineData(".sql")]
		[InlineData("migration#.sql")]
		[InlineData("migration-.sql")]
		[InlineData("migration.sequel")]
		[InlineData("migration.exe")]
		[InlineData("20210506185800_migration.docx")]
		[InlineData("20210506185800.txt")]
		[InlineData("_.xlsx")]
		public void MigrationNameValidatorTests__Validate__WhenInvalidFilenameIsGiven_ThenFalseReturned(string filename)
		{
			// Arrange
			var migrationNameValidator = new MigrationNameValidator();

			// Act
			var validationResult = migrationNameValidator.Validate(filename);

			// Assert
			Assert.False(validationResult);
		}

		[Theory]
		[InlineData("migration.sql")]
		[InlineData("20210506185800_migration.sql")]
		[InlineData("20210506185800.sql")]
		[InlineData("_.sql")]
		public void MigrationNameValidatorTests__Validate__WhenValidFilenameIsGiven_ThenTrueReturned(string filename)
		{
			// Arrange
			var migrationNameValidator = new MigrationNameValidator();

			// Act
			var validationResult = migrationNameValidator.Validate(filename);

			// Assert
			Assert.True(validationResult);
		}
	}
}
