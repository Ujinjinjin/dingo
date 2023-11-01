namespace Dingo.Core.Validators.Primitive;

internal sealed class StringRequiredValidator : IValidator<string?>
{
	public bool Validate(string? entity)
	{
		return !string.IsNullOrWhiteSpace(entity);
	}
}
