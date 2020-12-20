using Dingo.Core.Serializers;
using Dingo.UnitTests.Models;
using Xunit;

namespace Dingo.UnitTests.SerializerTests
{
	public class JsonInternalSerializerTests : UnitTestsBase
	{
		[Fact]
		public void JsonInternalSerializerTests__Serialize__WhenValidObjectGiven_ThenObjectSerialized()
		{
			// Arrange
			var jsonInternalSerializer = new JsonInternalSerializer();
			var data = new TestStruct
			{
				Property1 = "Value 1",
				Property2 = "Value 2",
			};
			var expectedSerializedData = "{\n  \"Property1\": \"Value 1\",\n  \"Property2\": \"Value 2\"\n}";

			// Act
			var serializedData = jsonInternalSerializer.Serialize(data);

			// Assert
			Assert.Equal(expectedSerializedData, serializedData);
		}

		[Fact]
		public void JsonInternalSerializerTests__Serialize__WhenStructWithNullValuePropertiesGiven_ThenObjectSerializedWithoutNullValues()
		{
			// Arrange
			var jsonInternalSerializer = new JsonInternalSerializer();
			var data = new TestStruct
			{
				Property1 = "Value 1",
				Property2 = null,
			};
			var expectedSerializedData = "{\n  \"Property1\": \"Value 1\"\n}";

			// Act
			var serializedData = jsonInternalSerializer.Serialize(data);

			// Assert
			Assert.Equal(expectedSerializedData, serializedData);
		}

		[Fact]
		public void JsonInternalSerializerTests__Deserialize__WhenSerializedDataWithMissingOfPropertiesGiven_ThenObjectDeserializedAndMissingPropertiesAreNull()
		{
			// Arrange
			var jsonInternalSerializer = new JsonInternalSerializer();
			var serializedData = "{\n  \"Property1\": \"Value 1\"\n}";
			var expectedData = new TestStruct
			{
				Property1 = "Value 1",
				Property2 = null
			};

			// Act
			var data = jsonInternalSerializer.Deserialize<TestStruct>(serializedData);

			// Assert
			Assert.Equal(expectedData, data);
		}

		[Fact]
		public void JsonInternalSerializerTests__Deserialize__WhenSufficientSerializedDataGiven_ThenObjectDeserializedWithAllProperties()
		{
			// Arrange
			var jsonInternalSerializer = new JsonInternalSerializer();
			var serializedData = "{\n  \"Property1\": \"Value 1\",\n  \"Property2\": \"Value 2\"\n}";
			var expectedData = new TestStruct
			{
				Property1 = "Value 1",
				Property2 = "Value 2"
			};

			// Act
			var data = jsonInternalSerializer.Deserialize<TestStruct>(serializedData);

			// Assert
			Assert.Equal(expectedData, data);
		}
	}
}
