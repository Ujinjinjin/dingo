namespace Dingo.Core.Validators;

/// <inheritdoc />
internal abstract class Validator<T> : IValidator<T>
{
	/// <summary> List of validation rules </summary>
	protected IList<Func<T, bool>> ValidationRules;

	/// <inheritdoc />
	public bool Validate(T entity)
	{
		if (ValidationRules == null || ValidationRules.Count == 0)
		{
			return true;
		}

		var result = true;

		for (var i = 0; i < ValidationRules.Count; i++)
		{
			result = result && ValidationRules[i](entity);
		}

		return result;
	}
}
