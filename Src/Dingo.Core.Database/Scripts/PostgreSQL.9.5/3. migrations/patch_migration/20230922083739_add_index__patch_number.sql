-- up
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_index('patch_migration', 'i$patch_migration$patch_number','dingo') is false) then
		create index i$patch_migration$patch_number on dingo.patch_migration (patch_number);
	end if;
end $$;

-- down
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_index('patch_migration', 'i$patch_migration$patch_number','dingo') is true) then
		select dingo.drop_index('patch_migration', 'i$patch_migration$patch_number','dingo');
	end if;
end $$;
