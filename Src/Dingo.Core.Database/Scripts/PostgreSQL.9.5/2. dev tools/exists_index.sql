create or replace function dingo.exists_index(
	p_table_name  varchar(128),
	p_index_name  varchar(512),
	p_schema_name varchar(128) = 'public'
)
returns boolean as
$$
declare v_index_exists boolean;
begin
	---------------	-------------------------------------------------
	select into v_index_exists exists (
		select from pg_indexes
		where 1 = 1
			and pg_indexes.schemaname = p_schema_name
			and pg_indexes.tablename  = p_table_name
			and pg_indexes.indexname  = p_index_name
	);
	return v_index_exists;
	----------------------------------------------------------------
end;
$$
language plpgsql;