using Dingo.Core.Repository.DbConverters;

namespace Dingo.Core.Factories;

/// <summary> Database contract converter factory. Creates <see cref="IDatabaseContextFactory"/> </summary>
internal interface IDatabaseContractConverterFactory
{
	/// <summary> Creates instance of <see cref="PostgresContractConverter"/> </summary>
	public IDatabaseContractConverter CreatePostgresContractConverter();

	/// <summary> Creates instance of <see cref="SqlServerContractConverter"/> </summary>
	public IDatabaseContractConverter CreateSqlServerContractConverter();
}