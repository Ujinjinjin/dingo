namespace Dingo.Core.New.Factories;

internal interface IMigrationFactory
{
	Migration Create(string up, string down);
}
