-- Drop type
select system__drop_type('t_migration_info_input');

-- Create type
create type t_migration_info_input as (
	migration_hash varchar(256),
	migration_path text
);

-- Set comments
comment on type t_migration_info_input is 'migration info input';
comment on column t_migration_info_input.migration_hash is 'migration hash';
comment on column t_migration_info_input.migration_path is 'migration file path';
