-- up
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_table('migration', 'dingo') is false) then
		create table migration(
			migration_id serial not null,
			migration_path text not null,
			migration_hash varchar(256) not null,
			date_updated timestamp not null
		);

		alter table migration add constraint pk$migration primary key (migration_id);
	end if;
end $$;

-- down
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_table('migration', 'dingo') is true) then
		drop table migration;
	end if;
end $$;
