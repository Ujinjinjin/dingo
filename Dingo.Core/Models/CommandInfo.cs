using Dingo.Abstractions;
using System.CommandLine;

namespace Dingo.Core.Models
{
	internal struct CommandInfo
	{
		public StackType StackType { get; set; }
		public Command Command { get; set; }
	}
}
