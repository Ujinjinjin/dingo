if dbo.system__exists_table('dingo_migration', default) = 0
begin
	create table dingo_migration(
		dingo_migration_id int not null identity(1, 1),
		migration_path nvarchar(max) not null,
		migration_hash varchar(256) not null,
		date_updated datetime not null,
		constraint pk$dingo_migration primary key clustered(dingo_migration_id asc)
	);
end
