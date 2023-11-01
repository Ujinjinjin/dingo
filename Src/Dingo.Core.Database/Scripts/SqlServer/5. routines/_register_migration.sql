create or alter procedure dingo._register_migration(
	@p_migration_path nvarchar(max),
	@p_migration_hash varchar(256),
	@p_date_updated datetime
)
as
begin
	----------------------------------------------------------------
	insert into dingo.migration (
		migration_path,
		migration_hash,
		date_updated
	) values (
		@p_migration_path,
		@p_migration_hash,
		@p_date_updated
	);
	----------------------------------------------------------------
end;
