namespace Dingo.Core.Services.Handlers;

public interface IConnectionHandler
{
	/// <summary> Handshake DB connection </summary>
	Task HandshakeAsync(string? profile, CancellationToken ct = default);
}
