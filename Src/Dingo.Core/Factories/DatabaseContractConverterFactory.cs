using Dingo.Core.Repository.DbConverters;

namespace Dingo.Core.Factories
{
	/// <inheritdoc />
	internal sealed class DatabaseContractConverterFactory : IDatabaseContractConverterFactory
	{
		/// <inheritdoc />
		public IDatabaseContractConverter CreatePostgresContractConverter()
		{
			return new PostgresContractConverter();
		}

		/// <inheritdoc />
		public IDatabaseContractConverter CreateSqlServerContractConverter()
		{
			return new SqlServerContractConverter();
		}
	}
}
