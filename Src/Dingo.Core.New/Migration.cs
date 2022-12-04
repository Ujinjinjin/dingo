using Dingo.Core.Validators.MigrationValidators;

namespace Dingo.Core;

public readonly struct Migration
{
	internal readonly string UpSql;
	internal readonly string DownSql;

	private readonly IMigrationValidator _validator;

	internal Migration(string up, string down, IMigrationValidator validator)
	{
		UpSql = up;
		DownSql = down;
		_validator = validator;
	}

	public void Up()
	{
		UpAsync().GetAwaiter().GetResult();
	}

	public async Task UpAsync()
	{
		await Task.CompletedTask;
	}

	public void Down()
	{
		DownAsync().GetAwaiter().GetResult();
	}

	public async Task DownAsync()
	{
		await Task.CompletedTask;
	}

	public bool IsValid()
	{
		return _validator.Validate(this);
	}
}
