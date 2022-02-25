using Dingo.Core.Repository;

namespace Dingo.Core.Factories;

/// <summary> Database context factory </summary>
internal interface IDatabaseContextFactory
{
	/// <summary> Create database context </summary>
	/// <returns> Instance of database context </returns>
	IDatabaseContext CreateDatabaseContext();
}