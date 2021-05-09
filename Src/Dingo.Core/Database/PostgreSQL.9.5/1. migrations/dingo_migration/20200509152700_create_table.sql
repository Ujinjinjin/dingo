do $$
begin
	if (select system__exists_table('dingo_migration') is false) then
		create table dingo_migration(
			dingo_migration_id serial not null,
			migration_path text not null,
			migration_hash varchar(256) not null,
			date_updated timestamp not null
		);

		alter table dingo_migration add constraint pk$dingo_migration primary key (dingo_migration_id);
	end if;
end $$