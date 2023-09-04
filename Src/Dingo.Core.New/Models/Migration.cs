using Dingo.Core.Migrations;
using Dingo.Core.Validators.Migration;

namespace Dingo.Core.Models;

public sealed class Migration : IMigrationMeta, IMigrationBody
{
	public MigrationPath Path { get; }
	public Hash Hash { get; }

	public MigrationCommand Command { get; }
	public MigrationStatus Status { get; set; }

	internal Migration(MigrationPath path, Hash hash, MigrationCommand command)
	{
		Path = path;
		Hash = hash;
		Command = command;
		Status = MigrationStatus.Unknown;
	}

	public static Migration Empty => new(default, default, default);

	internal void Apply(IMigrationApplier applier)
	{
		applier.Apply(this);
	}

	internal bool IsValid(IMigrationValidator validator)
	{
		return validator.Validate(this);
	}
}
