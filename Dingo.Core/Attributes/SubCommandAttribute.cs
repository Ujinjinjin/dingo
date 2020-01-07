using Dingo.Abstractions;
using System;

namespace Dingo.Core.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class SubCommandAttribute : Attribute
	{
		public string Name { get; }
		public string Description { get; }
		public StackType StackType { get; }

		public SubCommandAttribute(string name, string description = null, StackType stackType = StackType.Nested)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description;
			StackType = stackType;
		}
	}
}
