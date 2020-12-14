-- Drop type if exists
do $$
begin
	if exists (select 1 from pg_type where typname = 't_migration_info_input') then
		drop type t_migration_info_input;
	end if;
end$$;

-- Create type
create type t_migration_info_input as (
	migration_path text,
	migration_hash text
);