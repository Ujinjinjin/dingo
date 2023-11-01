using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using Dingo.Core.Extensions;

namespace Dingo.Core.Utils;

internal readonly struct CodeTiming : IDisposable
{
	private readonly ILogger _logger;
	private readonly long _startTicks;
	private readonly string? _callerName;

	public CodeTiming(ILogger logger, [CallerMemberName] string? callerName = default)
	{
		_logger = logger.Required(nameof(logger));
		_startTicks = DateTime.UtcNow.Ticks;
		_callerName = callerName;
	}

	public void Dispose()
	{
		var finishTicks = DateTime.UtcNow.Ticks;
		_logger.LogDebug($"Method:{_callerName}; elapsed: {TimeSpan.FromTicks(finishTicks - _startTicks)}");
	}
}
