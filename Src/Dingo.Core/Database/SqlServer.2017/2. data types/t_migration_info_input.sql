-- Drop type
exec system__drop_type 't_migration_info_input', default;

-- Create type
create type t_migration_info_input as table (
	migration_path nvarchar(max),
	migration_hash nvarchar(256)
);

-- Set comments
exec system__set_comment_type 't_migration_info_input', 'migration info input', default;
exec system__set_comment_type_column 't_migration_info_input', 'migration_hash', 'migration hash', default;
exec system__set_comment_type_column 't_migration_info_input', 'migration_path', 'migration file path', default;
