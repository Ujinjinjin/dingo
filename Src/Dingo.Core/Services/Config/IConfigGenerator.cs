namespace Dingo.Core.Services.Config;

public interface IConfigGenerator
{
	void Generate(string? path = default, string? profile = default);
}
