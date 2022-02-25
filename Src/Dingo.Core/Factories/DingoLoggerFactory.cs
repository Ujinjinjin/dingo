using Dingo.Core.Config;
using Dingo.Core.Helpers;
using Dingo.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Dingo.Core.Factories;

/// <inheritdoc />
internal sealed class DingoLoggerFactory : ILoggerFactory
{
	private readonly IConfigWrapper _configWrapper;
	private readonly IPathHelper _pathHelper;
	private readonly IOutputQueueFactory _outputQueueFactory;

	public DingoLoggerFactory(
		IConfigWrapper configWrapper,
		IPathHelper pathHelper,
		IOutputQueueFactory outputQueueFactory
	)
	{
		_configWrapper = configWrapper ?? throw new ArgumentNullException(nameof(configWrapper));
		_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
		_outputQueueFactory = outputQueueFactory ?? throw new ArgumentNullException(nameof(outputQueueFactory));
	}

	/// <inheritdoc />
	public void Dispose()
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public ILogger CreateLogger(string categoryName)
	{
		return new FileLogger(
			categoryName,
			_configWrapper,
			_outputQueueFactory,
			_pathHelper
		);
	}

	/// <inheritdoc />
	public void AddProvider(ILoggerProvider provider)
	{
		throw new NotImplementedException();
	}
}