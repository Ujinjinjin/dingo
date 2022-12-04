using Dingo.Core.New.Validators.MigrationValidator;

namespace Dingo.Core.New;

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

	public Task UpAsync()
	{
		return Task.CompletedTask;
	}

	public void Down()
	{
		DownAsync().GetAwaiter().GetResult();
	}

	public Task DownAsync()
	{
		return Task.CompletedTask;
	}

	public bool IsValid()
	{
		return _validator.Validate(this);
	}
}
