-- Set comments
comment on table dingo.patch_migration is 'applied migration registry';
comment on column dingo.patch_migration.migration_hash is 'migration hash at time of application';
comment on column dingo.patch_migration.migration_id is 'unique migration ID';
comment on column dingo.patch_migration.patch_number is 'patch that includes applied migration';
