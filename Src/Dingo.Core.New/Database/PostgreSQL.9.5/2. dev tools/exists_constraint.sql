create or replace function dingo.exists_constraint(
	p_table_name      varchar(128),
	p_constraint_name varchar(512),
	p_schema_name     varchar(128) = 'public'
)
returns boolean as
$$
declare v_constraint_exists boolean;
begin
	----------------------------------------------------------------
	select into v_constraint_exists exists (
		select from information_schema.table_constraints
		where 1 = 1
			and information_schema.table_constraints.constraint_schema = p_schema_name
			and information_schema.table_constraints.table_schema      = p_schema_name
			and information_schema.table_constraints.table_name        = p_table_name
			and information_schema.table_constraints.constraint_name   = p_constraint_name
	);
	return v_constraint_exists;
	----------------------------------------------------------------
end;
$$
language plpgsql;
