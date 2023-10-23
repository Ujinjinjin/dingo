-- up
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_table('patch_migration', 'dingo') is false) then
		create table patch_migration(
			migration_hash varchar(256) not null,
			migration_id integer not null,
			patch_number integer not null
		);

		alter table patch_migration add constraint pk$patch_migration primary key (patch_number, migration_id);
	end if;
end $$;

-- down
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_table('patch_migration', 'dingo') is true) then
		drop table patch_migration;
	end if;
end $$;
