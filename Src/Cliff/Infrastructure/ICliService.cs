namespace Cliff.Infrastructure;

/// <summary> Cli service </summary>
public interface ICliService
{
	/// <summary> Service provider of an application </summary>
	IServiceProvider ServiceProvider { get; }

	/// <summary> Execute command from CLI </summary>
	/// <param name="args">Arguments passed to application</param>
	Task ExecuteAsync(string[] args);
}