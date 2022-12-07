using System.Runtime.Serialization;
using Dingo.Core.Serialization;

namespace Dingo.UnitTests.Serialization;

public class YamlSerializerTests : UnitTestBase
{
	[Fact]
	public void YamlSerializerTests__Serialize__WhenValidObjectGiven_ThenObjectSerialized()
	{
		// Arrange
		var yamlSerializer = new YamlSerializer();
		var data = Fixture.Create<TestStruct>();
		var expectedSerializedData = $"Property1: {data.Property1}\nProperty2: {data.Property2}";

		// Act
		var serializedData = yamlSerializer.Serialize(data);

		// Assert
		serializedData.Should().Be(expectedSerializedData);
	}

	[Fact]
	public void YamlSerializerTests__Serialize__WhenStructWithNullValuePropertiesGiven_ThenObjectSerializedWithoutNullValues()
	{
		// Arrange
		var yamlSerializer = new YamlSerializer();
		var data = new TestStruct
		{
			Property1 = Fixture.Create<string>(),
			Property2 = null,
		};
		var expectedSerializedData = $"Property1: {data.Property1}";

		// Act
		var serializedData = yamlSerializer.Serialize(data);

		// Assert
		serializedData.Should().Be(expectedSerializedData);
	}

	[Fact]
	public void YamlSerializerTests__Deserialize__WhenSerializedDataWithMissingOfPropertiesGiven_ThenObjectDeserializedAndMissingPropertiesAreNull()
	{
		// Arrange
		var yamlSerializer = new YamlSerializer();
		var expectedData = new TestStruct
		{
			Property1 = Fixture.Create<string>(),
			Property2 = null,
		};
		var serializedData = $"Property1: {expectedData.Property1}";

		// Act
		var data = yamlSerializer.Deserialize<TestStruct>(serializedData);

		// Assert
		data.Should().Be(expectedData);
	}

	[Fact]
	public void YamlSerializerTests__Deserialize__WhenSufficientSerializedDataGiven_ThenObjectDeserializedWithAllProperties()
	{
		// Arrange
		var yamlSerializer = new YamlSerializer();
		var expectedData = Fixture.Create<TestStruct>();
		var serializedData = $"Property1: {expectedData.Property1}\nProperty2: {expectedData.Property2}";

		// Act
		var data = yamlSerializer.Deserialize<TestStruct>(serializedData);

		// Assert
		data.Should().Be(expectedData);
	}

	[Fact]
	public void YamlSerializerTests__Deserialize__WhenEmptySerializedDataGiven_ThenExceptionThrown()
	{
		// Arrange
		var yamlSerializer = new YamlSerializer();
		var serializedData = string.Empty;
		var action = () => yamlSerializer.Deserialize<TestStruct>(serializedData);

		// Act & Assert
		action.Should().Throw<SerializationException>();
	}
}
