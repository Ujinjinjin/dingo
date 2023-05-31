create or replace function dingo.drop_default(
	p_table_name  varchar(128),
	p_column_name varchar(512),
	p_schema_name varchar(128) = 'public'
)
returns text as
$$
declare
	r record;
	v_dropped_count smallint := 0;
begin
	----------------------------------------------------------------
	if (select dingo.exists_column(p_table_name, p_column_name, p_schema_name)) then
		for r in select
			information_schema.columns.table_schema as schema_name,
			information_schema.columns.table_name,
			information_schema.columns.column_name
		from information_schema.columns
		where 1 = 1
			and information_schema.columns.table_schema = p_schema_name
			and information_schema.columns.table_name   = p_table_name
			and information_schema.columns.column_name  = p_column_name
		loop
			execute 'alter table ' || quote_ident(r.schema_name) || '.' || quote_ident(r.table_name) || ' alter column ' || quote_ident(r.column_name) || 'drop default;';
			v_dropped_count = v_dropped_count + 1;
		end loop;
	end if;
	----------------------------------------------------------------
	return 'dropped ' || v_dropped_count || ' defaults';
	----------------------------------------------------------------
end;
$$
language plpgsql;