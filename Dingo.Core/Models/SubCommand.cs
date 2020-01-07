using Dingo.Abstractions;
using System.CommandLine;

namespace Dingo.Core.Models
{
	internal struct SubCommand
	{
		public StackType StackType { get; set; }
		public Command Command { get; set; }
	}
}
