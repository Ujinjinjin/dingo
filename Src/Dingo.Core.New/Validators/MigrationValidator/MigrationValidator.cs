namespace Dingo.Core.New.Validators.MigrationValidator;

internal class MigrationValidator : Validator<Migration>, IMigrationValidator
{
	private readonly IEnumerable<Validator<Migration>> _validators;

	public MigrationValidator(IEnumerable<Validator<Migration>> validators)
	{
		_validators = validators ?? throw new ArgumentNullException(nameof(validators));
	}

	protected override IReadOnlyList<Func<Migration, bool>> GetValidationRules()
	{
		var validationRules = new List<Func<Migration, bool>>();

		foreach (var validator in _validators)
		{
			if (validator is MigrationValidator)
			{
				continue;
			}

			validationRules.Add(m => validator.Validate(m));
		}

		return validationRules;
	}
}
