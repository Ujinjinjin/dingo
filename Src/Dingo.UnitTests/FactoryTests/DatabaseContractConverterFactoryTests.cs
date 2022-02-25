using AutoFixture;
using Dingo.Core.Factories;
using Dingo.Core.Repository.DbConverters;
using Xunit;

namespace Dingo.UnitTests.FactoryTests;

public class DatabaseContractConverterFactoryTests : UnitTestsBase
{
	[Fact]
	public void DatabaseContractConverterFactoryTests__CreatePostgresContractConverter__WhenFactoryMethodInvoked_ThenPostgresContractConverterReturned()
	{
		// Arrange
		var fixture = CreateFixture();
		var databaseContractConverterFactory = fixture.Create<DatabaseContractConverterFactory>();

		// Act
		var postgresContractConverter = databaseContractConverterFactory.CreatePostgresContractConverter();

		// Assert
		Assert.NotNull(postgresContractConverter);
		Assert.IsAssignableFrom<IDatabaseContractConverter>(postgresContractConverter);
		Assert.Equal(typeof(PostgresContractConverter), postgresContractConverter.GetType());
	}

	[Fact]
	public void DatabaseContractConverterFactoryTests__CreateSqlServerContractConverter__WhenFactoryMethodInvoked_ThenSqlServerContractConverterReturned()
	{
		// Arrange
		var fixture = CreateFixture();
		var databaseContractConverterFactory = fixture.Create<DatabaseContractConverterFactory>();

		// Act
		var sqlServerContractConverter = databaseContractConverterFactory.CreateSqlServerContractConverter();

		// Assert
		Assert.NotNull(sqlServerContractConverter);
		Assert.IsAssignableFrom<IDatabaseContractConverter>(sqlServerContractConverter);
		Assert.Equal(typeof(SqlServerContractConverter), sqlServerContractConverter.GetType());
	}
}