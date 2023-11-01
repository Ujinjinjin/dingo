namespace Dingo.Core.Services.Handlers;

public interface IConfigHandler
{
	/// <summary> Create configuration profile </summary>
	void Init(string? path = default, string? profile = default);
}
