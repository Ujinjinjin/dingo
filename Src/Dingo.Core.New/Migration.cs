using Dingo.Core.Validators.MigrationValidators;

namespace Dingo.Core;

public readonly struct Migration
{
	internal readonly string UpSql;
	internal readonly string DownSql;

	internal Migration(string up, string down)
	{
		UpSql = up;
		DownSql = down;
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

	internal bool IsValid(IMigrationValidator validator)
	{
		return validator.Validate(this);
	}
}
