-- up
declare @system_patch_number int = 1;
declare @system_patch int = 1;
update dingo.patch
	set [type] = @system_patch
	where patch_number = @system_patch_number;

-- down
declare @system_patch_number int = 1;
declare @default_patch_type int = 2;
update dingo.patch
	set [type] = @default_patch_type
	where patch_number = @system_patch_number;
