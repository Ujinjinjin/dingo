namespace Dingo.Core.Services.Config;

public interface IConfigProfileLoader
{
	Task LoadAsync(string? profile, CancellationToken ct = default);
}
