using Dingo.Core.Serialization;

namespace Dingo.UnitTests.Serialization;

public class SerializerFactoryTests : UnitTestBase
{
	[Theory]
	[InlineData("json")]
	[InlineData("yaml")]
	[InlineData("yml")]
	public void SerializerFactoryTests_Create__WhenSupportedExtensionGiven_ThenSerializerCreated(string extension)
	{
		// arrange
		var name = Fixture.Create<string>();
		var filename = $"{name}.{extension}";
		var factory = new SerializerFactory();

		// act
		var serializer = factory.Create(filename);

		// assert
		serializer.Should().NotBeNull();
		serializer.Should().BeAssignableTo<ISerializer>();
	}

	[Fact]
	public void SerializerFactoryTests_Create__WhenInvalidExtensionGiven_ThenExceptionThrown()
	{
		// arrange
		var name = Fixture.Create<string>();
		var extension = Fixture.Create<string>();
		var filename = $"{name}.{extension}";
		var factory = new SerializerFactory();

		// act
		var action = () => factory.Create(filename);

		// assert
		action.Should().Throw<ArgumentOutOfRangeException>();
	}

	[Fact]
	public void SerializerFactoryTests_Create__WhenNoExtensionGiven_ThenExceptionThrown()
	{
		// arrange
		var name = Fixture.Create<string>();
		var filename = $"{name}";
		var factory = new SerializerFactory();

		// act
		var action = () => factory.Create(filename);

		// assert
		action.Should().Throw<ArgumentOutOfRangeException>();
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void SerializerFactoryTests_Create__WhenEmptyExtensionGiven_ThenExceptionThrown(string extension)
	{
		// arrange
		var name = Fixture.Create<string>();
		var filename = $"{name}.{extension}";
		var factory = new SerializerFactory();

		// act
		var action = () => factory.Create(filename);

		// assert
		action.Should().Throw<ArgumentOutOfRangeException>();
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void SerializerFactoryTests_Create__WhenEmptyFilenameGiven_ThenExceptionThrown(string filename)
	{
		// arrange
		var factory = new SerializerFactory();

		// act
		var action = () => factory.Create(filename);

		// assert
		action.Should().Throw<ArgumentNullException>();
	}
}
