namespace Dingo.Core.Services.Handlers;

public interface IConnectionHandler
{
	/// <summary> Handshake DB connection </summary>
	Task HandshakeAsync(CancellationToken ct = default);
}
