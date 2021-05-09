create or replace function system__drop_type(
	p_type_name   varchar(512),
	p_schema_name varchar(128) = 'public'
)
returns text as
$$
declare
	r record;
	v_dropped_count smallint := 0;
begin
	----------------------------------------------------------------
	if (select system__exists_type(p_type_name, p_schema_name)) then
		for r in select
			information_schema.user_defined_types.user_defined_type_schema as schema_name,
			information_schema.user_defined_types.user_defined_type_name   as type_name
		from information_schema.user_defined_types
		where 1 = 1
			and information_schema.user_defined_types.user_defined_type_schema = p_schema_name
			and information_schema.user_defined_types.user_defined_type_name   = p_type_name
		loop
			execute 'drop type ' || quote_ident(r.schema_name) || '.' || quote_ident(r.type_name) || ' cascade;';
			v_dropped_count = v_dropped_count + 1;
		end loop;
	end if;
	----------------------------------------------------------------
	return 'dropped ' || v_dropped_count || ' types';
	----------------------------------------------------------------
end;
$$
language plpgsql;