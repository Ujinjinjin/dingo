using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace Dingo.Core.Utils
{
	public readonly struct CodeTiming : IDisposable
	{
		private readonly ILogger _logger;
		private readonly long _startTicks;
		private readonly string _callerName;

		public CodeTiming(ILogger logger, [CallerMemberName] string callerName = null)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_startTicks = DateTime.UtcNow.Ticks;
			_callerName = callerName;
		}

		public void Dispose()
		{
			var finishTicks = DateTime.UtcNow.Ticks;
			_logger.LogDebug($"Method:{_callerName}; elapsed: {TimeSpan.FromTicks(finishTicks - _startTicks)}");
		}
	}
}
