-- Drop type
exec dingo.drop_type 't_migration_info_input', 'dingo';

-- Create type
create type dingo.t_migration_info_input as table (
	migration_hash nvarchar(256),
	migration_path nvarchar(512)
);

-- Set comments
exec dingo.set_comment_type 't_migration_info_input', 'migration info input', 'dingo';
exec dingo.set_comment_type_column 't_migration_info_input', 'migration_hash', 'migration hash', 'dingo';
exec dingo.set_comment_type_column 't_migration_info_input', 'migration_path', 'migration file path', 'dingo';
