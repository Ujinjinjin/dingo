-- Drop type
select dingo.drop_type('t_migration_info_input', 'dingo');

-- Create type
create type dingo.t_migration_info_input as (
	migration_hash varchar(256),
	migration_path varchar(512)
);

-- Set comments
comment on type dingo.t_migration_info_input is 'migration info input';
comment on column dingo.t_migration_info_input.migration_hash is 'migration hash';
comment on column dingo.t_migration_info_input.migration_path is 'migration file path';
