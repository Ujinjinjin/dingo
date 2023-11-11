create or replace function dingo.drop_constraint(
	p_table_name      varchar(128),
	p_constraint_name varchar(512),
	p_schema_name     varchar(128) = 'public'
)
returns text as
$$
declare
	r record;
	v_dropped_count smallint := 0;
begin
	----------------------------------------------------------------
	if (select dingo.exists_constraint(p_table_name, p_constraint_name, p_schema_name)) then
		for r in select
			information_schema.table_constraints.table_schema as schema_name,
			information_schema.table_constraints.table_name,
			information_schema.table_constraints.constraint_name
		from information_schema.table_constraints
		where 1 = 1
			and information_schema.table_constraints.constraint_schema = p_schema_name
			and information_schema.table_constraints.table_schema      = p_schema_name
			and information_schema.table_constraints.table_name        = p_table_name
			and information_schema.table_constraints.constraint_name   = p_constraint_name
		loop
			execute 'alter table ' || quote_ident(r.schema_name) || '.' || quote_ident(r.table_name) || ' drop constraint ' || quote_ident(r.constraint_name) || ';';
			v_dropped_count = v_dropped_count + 1;
		end loop;
	end if;
	----------------------------------------------------------------
	return 'dropped ' || v_dropped_count || ' constraints';
	----------------------------------------------------------------
end;
$$
language plpgsql;