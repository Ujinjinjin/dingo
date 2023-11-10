-- up
if dingo.exists_table('patch_migration', 'dingo') = 0
begin
	create table dingo.patch_migration(
		migration_hash varchar(256) not null,
		migration_id integer not null,
		patch_number integer not null
		constraint pk$patch_migration primary key clustered(patch_number asc, migration_id asc)
	);
end

-- down
if dingo.exists_table('patch_migration', 'dingo') = 1
begin
	drop table dingo.patch_migration;
end
