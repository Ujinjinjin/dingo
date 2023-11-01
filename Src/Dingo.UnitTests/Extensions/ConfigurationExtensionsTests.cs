using Dingo.Core.Extensions;
using Trico.Configuration;

namespace Dingo.UnitTests.Extensions;

public class ConfigurationExtensionsTests : UnitTestBase
{
	[Theory]
	[InlineData("true")]
	[InlineData("True")]
	[InlineData("TrUE")]
	[InlineData("TRUE")]
	public void ConfigurationExtensionsTests_IsTrue__WhenConfigValueIsTrue_ThenTrueReturned(string value)
	{
		// arrange
		var config = SetupConfiguration(value);

		// act
		var res = config.IsTrue(Fixture.Create<string>());

		// assert
		res.Should().BeTrue();
	}

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData(null)]
	public void ConfigurationExtensionsTests_IsTrue__WhenConfigValueIsNullOrEmpty_ThenFalseReturned(string value)
	{
		// arrange
		var config = SetupConfiguration(value);

		// act
		var res = config.IsTrue(Fixture.Create<string>());

		// assert
		res.Should().BeFalse();
	}

	[Fact]
	public void ConfigurationExtensionsTests_IsTrue__WhenConfigValueIsNotTrue_ThenFalseReturned()
	{
		// arrange
		var config = SetupConfiguration(Fixture.Create<string>());

		// act
		var res = config.IsTrue(Fixture.Create<string>());

		// assert
		res.Should().BeFalse();
	}

	[Theory]
	[InlineData("false")]
	[InlineData("False")]
	[InlineData("falSe")]
	[InlineData("FALSE")]
	public void ConfigurationExtensionsTests_IsFalse__WhenConfigValueIsFalse_ThenTrueReturned(string value)
	{
		// arrange
		var config = SetupConfiguration(value);

		// act
		var res = config.IsFalse(Fixture.Create<string>());

		// assert
		res.Should().BeTrue();
	}

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData(null)]
	public void ConfigurationExtensionsTests_IsFalse__WhenConfigValueIsNullOrEmpty_ThenFalseReturned(string value)
	{
		// arrange
		var config = SetupConfiguration(value);

		// act
		var res = config.IsFalse(Fixture.Create<string>());

		// assert
		res.Should().BeFalse();
	}

	[Fact]
	public void ConfigurationExtensionsTests_IsFalse__WhenConfigValueIsNotFalse_ThenFalseReturned()
	{
		// arrange
		var config = SetupConfiguration(Fixture.Create<string>());

		// act
		var res = config.IsTrue(Fixture.Create<string>());

		// assert
		res.Should().BeFalse();
	}

	private IConfiguration SetupConfiguration(string value)
	{
		var config = new Mock<IConfiguration>();
		config.Setup(c => c.Get(It.IsAny<string>()))
			.Returns(value);

		return config.Object;
	}
}
