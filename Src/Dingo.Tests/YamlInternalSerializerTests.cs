using Dingo.Core.Serializers;
using Dingo.Tests.Models;
using System.Runtime.Serialization;
using Xunit;

namespace Dingo.Tests
{
	public class YamlInternalSerializerTests
	{
		[Fact]
		public void YamlInternalSerializerTests__Serialize__WhenValidObjectGiven_ThenObjectSerialized()
		{
			// Arrange
			var yamlInternalSerializer = new YamlInternalSerializer();
			var data = new TestStruct
			{
				Property1 = "Value 1",
				Property2 = "Value 2",
			};
			var expectedSerializedData = "Property1: Value 1\nProperty2: Value 2";

			// Act
			var serializedData = yamlInternalSerializer.Serialize(data);

			// Assert
			Assert.Equal(expectedSerializedData, serializedData);
		}
		
		[Fact]
		public void YamlInternalSerializerTests__Serialize__WhenStructWithNullValuePropertiesGiven_ThenObjectSerializedWithoutNullValues()
		{
			// Arrange
			var yamlInternalSerializer = new YamlInternalSerializer();
			var data = new TestStruct
			{
				Property1 = "Value 1",
				Property2 = null,
			};
			var expectedSerializedData = "Property1: Value 1";

			// Act
			var serializedData = yamlInternalSerializer.Serialize(data);

			// Assert
			Assert.Equal(expectedSerializedData, serializedData);
		}
		
		[Fact]
		public void YamlInternalSerializerTests__Deserialize__WhenSerializedDataWithMissingOfPropertiesGiven_ThenObjectDeserializedAndMissingPropertiesAreNull()
		{
			// Arrange
			var yamlInternalSerializer = new YamlInternalSerializer();
			var serializedData = "Property1: Value 1";

			var expectedData = new TestStruct
			{
				Property1 = "Value 1",
				Property2 = null
			};

			// Act
			var data = yamlInternalSerializer.Deserialize<TestStruct>(serializedData);

			// Assert
			Assert.Equal(expectedData, data);
		}
		
		[Fact]
		public void YamlInternalSerializerTests__Deserialize__WhenSufficientSerializedDataGiven_ThenObjectDeserializedWithAllProperties()
		{
			// Arrange
			var yamlInternalSerializer = new YamlInternalSerializer();
			var serializedData = "Property1: Value 1\nProperty2: Value 2";
			var expectedData = new TestStruct
			{
				Property1 = "Value 1",
				Property2 = "Value 2"
			};

			// Act
			var data = yamlInternalSerializer.Deserialize<TestStruct>(serializedData);

			// Assert
			Assert.Equal(expectedData, data);
		}
		
		[Fact]
		public void YamlInternalSerializerTests__Deserialize__WhenEmptySerializedDataGiven_ThenExceptionThrown()
		{
			// Arrange
			var yamlInternalSerializer = new YamlInternalSerializer();
			var serializedData = "";

			// Act & Assert
			Assert.Throws<SerializationException>(() => yamlInternalSerializer.Deserialize<TestStruct>(serializedData));
		}
	}
}
