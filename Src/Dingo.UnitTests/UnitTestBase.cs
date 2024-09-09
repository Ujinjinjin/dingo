using System.Data;
using Dingo.Core.Models;
using Dingo.Core.Repository;
using Dingo.Core.Repository.Models;
using Dingo.UnitTests.Helpers;
using Microsoft.Extensions.Logging;

namespace Dingo.UnitTests;

public class UnitTestBase
{
	protected readonly Fixture Fixture;

	protected UnitTestBase()
	{
		Fixture = new Fixture();
	}

	protected Migration CreateMigration(MigrationPath path, Hash hash, MigrationCommand command)
	{
		return new Migration(path, hash, command);
	}

	protected Migration CreateMigration(
		MigrationStatus status = MigrationStatus.None,
		string? hash = default
	)
	{
		Fixture.Register(() => new Hash(hash ?? Fixture.Create<string>()));
		Fixture.Register(
			() => new Migration(
				Fixture.Create<MigrationPath>(),
				Fixture.Create<Hash>(),
				Fixture.Create<MigrationCommand>()
			) { Status = status }
		);

		return Fixture.Create<Migration>();
	}


	protected IReadOnlyList<Migration> CreateMigrations(
		string hash,
		int? count = null
	)
	{
		count ??= 3;
		Fixture.Register(() => new Hash(hash));
		Fixture.Register(
			() => new Migration(
				Fixture.Create<MigrationPath>(),
				Fixture.Create<Hash>(),
				Fixture.Create<MigrationCommand>()
			)
		);

		return Fixture.CreateMany<Migration>(count.Value).ToArray();
	}

	protected IReadOnlyList<MigrationComparisonOutput> CreateMigrationComparisonOutput(
		bool? hashMatches,
		string? hash,
		int? count = null
	)
	{
		count ??= 3;
		Fixture.Register(
			() => new MigrationComparisonOutput
			{
				MigrationHash = hash ?? Fixture.Create<string>(),
				HashMatches = hashMatches,
			}
		);

		return Fixture.CreateMany<MigrationComparisonOutput>(count.Value).ToArray();
	}

	protected PatchMigration CreatePatchMigration(string? path = default, string? hash = default)
	{
		Fixture.Register(
			() => new PatchMigration(
				hash ?? Fixture.Create<string>(),
				new MigrationPath(
					Fixture.Create<string>(),
					path ?? Fixture.Create<string>(),
					Fixture.Create<string>(),
					Fixture.Create<string>()
				),
				Fixture.Create<int>()
			)
		);

		return Fixture.Create<PatchMigration>();
	}

	protected ILoggerFactory SetupLoggerFactory()
	{
		var factory = new Mock<ILoggerFactory>();
		var logger = new Mock<ILogger>();

		factory.Setup(f => f.CreateLogger(It.IsAny<string>()))
			.Returns(logger.Object);

		return factory.Object;
	}

	protected IConnectionFactory SetupConnectionFactory(IDbConnection connection)
	{
		var factory = new Mock<IConnectionFactory>();
		factory.Setup(f => f.Create())
			.Returns(() => new DummyConnection(connection));

		return factory.Object;
	}

	protected IDbConnection MockConnection(ConnectionState state, bool succeed = true)
	{
		var connection = new Mock<IDbConnection>();

		connection.Setup(c => c.State)
			.Returns(state);

		if (!succeed)
		{
			connection.Setup(c => c.Open())
				.Throws<Exception>();
		}

		return connection.Object;
	}
}
