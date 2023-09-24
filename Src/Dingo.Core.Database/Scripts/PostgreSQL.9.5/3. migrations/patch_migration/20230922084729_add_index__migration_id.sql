-- up
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_index('patch_migration', 'i$patch_migration$migration_id','dingo') is false) then
		create index i$patch_migration$migration_id on dingo.patch_migration (migration_id);
	end if;
end $$;

-- down
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_index('patch_migration', 'i$patch_migration$migration_id','dingo') is true) then
		select dingo.drop_index('patch_migration', 'i$patch_migration$migration_id','dingo');
	end if;
end $$;
