-- up
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_index('migration', 'ui$migration$migration_path', 'dingo') is false) then
		create unique index ui$migration$migration_path on dingo.migration (migration_path);
	end if;
end $$;

-- down
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_index('migration', 'ui$migration$migration_path', 'dingo') is true) then
		select dingo.drop_index('migration', 'ui$migration$migration_path', 'dingo');
	end if;
end $$;
