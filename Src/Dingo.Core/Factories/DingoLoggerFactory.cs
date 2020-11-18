using Dingo.Core.Helpers;
using Dingo.Core.Logging;
using Microsoft.Extensions.Logging;
using System;

namespace Dingo.Core.Factories
{
	/// <inheritdoc />
	internal class DingoLoggerFactory : ILoggerFactory
	{
		private const LogLevel LogLevel = Microsoft.Extensions.Logging.LogLevel.Debug;

		private readonly IPathHelper _pathHelper;
		private readonly IOutputQueueFactory _outputQueueFactory;

		public DingoLoggerFactory(IPathHelper pathHelper, IOutputQueueFactory outputQueueFactory)
		{
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
			return new FileLogger(categoryName, LogLevel, _outputQueueFactory, _pathHelper);
		}

		/// <inheritdoc />
		public void AddProvider(ILoggerProvider provider)
		{
			throw new NotImplementedException();
		}
	}
}
