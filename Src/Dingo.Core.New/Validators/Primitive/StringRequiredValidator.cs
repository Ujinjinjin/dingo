namespace Dingo.Core.New.Validators.Primitive;

internal class StringRequiredValidator : IValidator<string>
{
	public bool Validate(string entity)
	{
		return !string.IsNullOrWhiteSpace(entity);
	}
}
