using System;
using System.Text.RegularExpressions;

namespace Dingo.Core.Validators;

/// <summary> Migration filename validator </summary>
internal sealed class MigrationNameValidator : Validator<string>
{
	private readonly Regex _regex = new(@"^[\w\d_]+\.sql$");

	public MigrationNameValidator()
	{
		ValidationRules = new Func<string, bool>[]
		{
			x => !string.IsNullOrWhiteSpace(x),
			x => _regex.IsMatch(x)
		};
	}
}