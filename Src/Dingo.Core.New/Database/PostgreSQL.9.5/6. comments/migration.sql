-- Set comments
comment on table dingo.migration is 'applied migration registry';
comment on column dingo.migration.date_updated is 'timestamp when migration was applied';
comment on column dingo.migration.migration_id is 'unique migration ID';
comment on column dingo.migration.migration_hash is 'hash sum of migration file';
comment on column dingo.migration.migration_path is 'relative path to migration file';
