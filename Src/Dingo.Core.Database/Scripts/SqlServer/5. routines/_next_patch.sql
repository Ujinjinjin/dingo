create or alter procedure dingo._next_patch
as
begin
	----------------------------------------------------------------
	declare @v_patch_number as integer;
	declare @v_last_patch_empty as bit;
	----------------------------------------------------------------
	select top 1
		@v_patch_number = patch.patch_number
	from dingo.patch as patch
	order by patch.patch_number desc;
	----------------------------------------------------------------
	-- if there are no patches in the table, create one
	if (@v_patch_number is null)
	begin
		insert into dingo.patch default values;
		set @v_patch_number = scope_identity();
	end;
	----------------------------------------------------------------
	set @v_last_patch_empty = 1;
	----------------------------------------------------------------
	select top 1
		@v_last_patch_empty = 0
	from dingo.patch_migration as patch_migration
	where patch_migration.patch_number = @v_patch_number;
	----------------------------------------------------------------
	-- create new patch only if the last one is not empty
	if (@v_last_patch_empty = 0)
	begin
		insert into dingo.patch default values;
		set @v_patch_number = scope_identity();
	end;
	----------------------------------------------------------------
	select @v_patch_number;
	----------------------------------------------------------------
end;