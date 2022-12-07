using Dingo.Core.Serialization;

namespace Dingo.UnitTests.Serialization;

public class JsonSerializerTests : UnitTestBase
{
	[Fact]
	public void JsonSerializerTests__Serialize__WhenValidObjectGiven_ThenObjectSerialized()
	{
		// Arrange
		var jsonSerializer = new JsonSerializer();
		var data = Fixture.Create<TestStruct>();
		var expectedSerializedData = $"{{\n  \"Property1\": \"{data.Property1}\",\n  \"Property2\": \"{data.Property2}\"\n}}";

		// Act
		var serializedData = jsonSerializer.Serialize(data);

		// Assert
		serializedData.Should().Be(expectedSerializedData);
	}

	[Fact]
	public void JsonSerializerTests__Serialize__WhenStructWithNullValuePropertiesGiven_ThenObjectSerializedWithoutNullValues()
	{
		// Arrange
		var jsonSerializer = new JsonSerializer();
		var data = new TestStruct
		{
			Property1 = Fixture.Create<string>(),
			Property2 = null,
		};
		var expectedSerializedData = $"{{\n  \"Property1\": \"{data.Property1}\"\n}}";

		// Act
		var serializedData = jsonSerializer.Serialize(data);

		// Assert
		serializedData.Should().Be(expectedSerializedData);
	}

	[Fact]
	public void JsonSerializerTests__Deserialize__WhenSerializedDataWithMissingOfPropertiesGiven_ThenObjectDeserializedAndMissingPropertiesAreNull()
	{
		// Arrange
		var jsonSerializer = new JsonSerializer();
		var expectedData = new TestStruct
		{
			Property1 = Fixture.Create<string>(),
			Property2 = null
		};
		var serializedData = $"{{\n  \"Property1\": \"{expectedData.Property1}\"\n}}";

		// Act
		var data = jsonSerializer.Deserialize<TestStruct>(serializedData);

		// Assert
		data.Should().Be(expectedData);
	}

	[Fact]
	public void JsonSerializerTests__Deserialize__WhenSufficientSerializedDataGiven_ThenObjectDeserializedWithAllProperties()
	{
		// Arrange
		var jsonSerializer = new JsonSerializer();
		var expectedData = new TestStruct
		{
			Property1 = Fixture.Create<string>(),
			Property2 = Fixture.Create<string>(),
		};
		var serializedData = $"{{\n  \"Property1\": \"{expectedData.Property1}\",\n  \"Property2\": \"{expectedData.Property2}\"\n}}";

		// Act
		var data = jsonSerializer.Deserialize<TestStruct>(serializedData);

		// Assert
		data.Should().Be(expectedData);
	}
}
