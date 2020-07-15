using Dingo.Core.Constants;
using Dingo.Core.Factories;
using Dingo.Core.Serializers;
using Dingo.UnitTests.Base;
using Xunit;

namespace Dingo.UnitTests
{
	public class InternalSerializerFactoryTests : UnitTestsBase
	{
		[Fact]
		public void InternalSerializerFactoryTests__CreateInternalSerializer__WhenJsonFilenameGiven_ThenJsonInternalSerializerReturned()
		{
			// Arrange
			var internalSerializerFactory = new InternalSerializerFactory();
			var filename = "dingo.json";

			// Act
			var internalSerializer = internalSerializerFactory.CreateInternalSerializer(filename);

			// Assert
			Assert.Equal(typeof(JsonInternalSerializer), internalSerializer.GetType());
			Assert.Equal(FileExtension.Json, internalSerializer.DefaultFileExtension);
		}
		
		[Fact]
		public void InternalSerializerFactoryTests__CreateInternalSerializer__WhenYmlFilenameGiven_ThenYamlInternalSerializerReturned()
		{
			// Arrange
			var internalSerializerFactory = new InternalSerializerFactory();
			var filename = "dingo.yml";

			// Act
			var internalSerializer = internalSerializerFactory.CreateInternalSerializer(filename);

			// Assert
			Assert.Equal(typeof(YamlInternalSerializer), internalSerializer.GetType());
			Assert.Equal(FileExtension.Yml, internalSerializer.DefaultFileExtension);
		}
		
		[Fact]
		public void InternalSerializerFactoryTests__CreateInternalSerializer__WhenYamlFilenameGiven_ThenYamlInternalSerializerReturned()
		{
			// Arrange
			var internalSerializerFactory = new InternalSerializerFactory();
			var filename = "dingo.yaml";

			// Act
			var internalSerializer = internalSerializerFactory.CreateInternalSerializer(filename);

			// Assert
			Assert.Equal(typeof(YamlInternalSerializer), internalSerializer.GetType());
			Assert.Equal(FileExtension.Yml, internalSerializer.DefaultFileExtension);
		}
	}
}
