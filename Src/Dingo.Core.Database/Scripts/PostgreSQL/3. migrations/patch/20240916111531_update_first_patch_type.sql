-- up
do $$
declare
	system_patch integer := 1;
	system_patch_number integer := 1;
begin
	set search_path to dingo;

	update patch
		set "type" = system_patch
		where patch_number = system_patch_number;
end $$;

-- down
do $$
declare
	default_patch_type integer := 2;
	system_patch_number integer := 1;
begin
	set search_path to dingo;

	update patch
		set "type" = default_patch_type
		where patch_number = system_patch_number;
end $$;
