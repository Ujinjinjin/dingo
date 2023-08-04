-- use system__drop_* functions to drop obsolete objects
exec dingo.drop_routine 'system__check_table_existence', default;
exec dingo.drop_routine 'system__get_migrations_status', default;
exec dingo.drop_routine 'system__register_migration', default;
