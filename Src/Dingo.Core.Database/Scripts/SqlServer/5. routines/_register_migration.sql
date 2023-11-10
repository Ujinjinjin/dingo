create or alter procedure dingo._register_migration(
	@p_migration_path varchar(max),
	@p_migration_hash varchar(256),
	@p_patch_number integer
)
as
begin
	----------------------------------------------------------------
	declare @v_migration_exists as bit;
	declare @v_migration_id as integer;
	----------------------------------------------------------------
	set @v_migration_exists = 0;
	select top 1
		@v_migration_exists = 1
	from dingo.migration
	where migration_path = @p_migration_path
	----------------------------------------------------------------
	if (@v_migration_exists = 0)
	begin
		insert into dingo.migration (migration_path) values (@p_migration_path);
		set @v_migration_id = scope_identity();
	end else
	begin
		select top 1
			@v_migration_id = migration_id
		from dingo.migration
		where migration_path = @p_migration_path;
	end;
	----------------------------------------------------------------
	insert into dingo.patch_migration (migration_hash, migration_id, patch_number) values
		(@p_migration_hash, @v_migration_id, @p_patch_number);
	----------------------------------------------------------------
end;
