using Dingo.Core.Validators.MigrationValidators;

namespace Dingo.Core.Migrations;

public readonly struct Migration
{
	internal readonly string? Up;
	internal readonly string? Down;

	internal Migration(string? up, string? down)
	{
		Up = up;
		Down = down;
	}

	public static Migration Empty => new(default, default);

	internal bool IsValid(IMigrationValidator validator)
	{
		return validator.Validate(this);
	}
}
