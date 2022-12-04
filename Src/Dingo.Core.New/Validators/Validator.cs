namespace Dingo.Core.Validators;

/// <inheritdoc />
internal abstract class Validator<T> : IValidator<T>
{
	/// <summary> List of validation rules </summary>
	private IReadOnlyList<Func<T, bool>>? _validationRules;

	protected abstract IReadOnlyList<Func<T, bool>> GetValidationRules();

	/// <inheritdoc />
	public bool Validate(T entity)
	{
		_validationRules = GetValidationRules();

		if (_validationRules == null || _validationRules.Count == 0)
		{
			return true;
		}

		for (var i = 0; i < _validationRules.Count; i++)
		{
			var result = _validationRules[i](entity);
			if (!result)
			{
				return false;
			}
		}

		return true;
	}
}
