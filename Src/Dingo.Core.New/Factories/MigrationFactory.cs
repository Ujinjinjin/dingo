using Dingo.Core.Validators.MigrationValidators;

namespace Dingo.Core.Factories;

internal sealed class MigrationFactory : IMigrationFactory
{
	private readonly IMigrationValidator _validator;

	public MigrationFactory(IMigrationValidator validator)
	{
		_validator = validator ?? throw new ArgumentNullException(nameof(validator));
	}

	public Migration Create(string up, string down)
	{
		return new Migration(up, down, _validator);
	}
}
