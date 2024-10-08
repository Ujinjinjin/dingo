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
		Status = MigrationStatus.None;
	}

	public static Migration Empty => new(default, default, default);

	internal bool IsValid(IMigrationValidator validator)
	{
		return validator.Validate(this);
	}
}
