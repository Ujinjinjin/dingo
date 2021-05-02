-- Drop type if exists
drop type if exists t_migration_info_input;

-- Create type
create type t_migration_info_input as table (
	migration_path nvarchar(max),
	migration_hash nvarchar(256)
);
