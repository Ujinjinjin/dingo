using Dingo.Core.New.Validators.MigrationValidator;

namespace Dingo.Core.New.Factories;

internal class MigrationFactory : IMigrationFactory
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
