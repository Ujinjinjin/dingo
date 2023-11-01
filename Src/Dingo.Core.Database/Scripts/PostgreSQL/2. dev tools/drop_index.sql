create or replace function dingo.drop_index(
	p_table_name  varchar(128),
	p_index_name  varchar(512),
	p_schema_name varchar(128) = 'public'
)
returns text as
$$
declare
	r record;
	v_dropped_count smallint := 0;
begin
	----------------------------------------------------------------
	if (select dingo.exists_index(p_table_name, p_index_name, p_schema_name)) then
		for r in select
			pg_indexes.schemaname as schema_name,
			pg_indexes.indexname  as index_name
		from pg_indexes
		where 1 = 1
			and pg_indexes.schemaname = p_schema_name
			and pg_indexes.tablename  = p_table_name
			and pg_indexes.indexname  = p_index_name
		loop
			execute 'drop index if exists ' || quote_ident(r.schema_name) || '.' || quote_ident(r.index_name) || ';';
			v_dropped_count = v_dropped_count + 1;
		end loop;
	end if;
	----------------------------------------------------------------
	return 'dropped ' || v_dropped_count || ' indices';
	----------------------------------------------------------------
end;
$$
language plpgsql;