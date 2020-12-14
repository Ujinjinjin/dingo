comment on table "dingo_migration" is 'User';
comment on column "dingo_migration".dingo_migration_id is 'Unique migration ID';
comment on column "dingo_migration".migration_path is 'Relative path to migration file';
comment on column "dingo_migration".migration_hash is 'Hash sum of migration file';
comment on column "dingo_migration".date_updated is 'Timestamp when migration was applied';