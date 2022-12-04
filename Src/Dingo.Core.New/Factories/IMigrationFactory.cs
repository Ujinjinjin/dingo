namespace Dingo.Core.Factories;

internal interface IMigrationFactory
{
	Migration Create(string up, string down);
}
