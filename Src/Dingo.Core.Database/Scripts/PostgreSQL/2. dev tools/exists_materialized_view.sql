create or replace function dingo.exists_materialized_view(
	p_mat_view_name varchar(128),
	p_schema_name   varchar(128) = 'public'
)
returns boolean as
$$
declare v_mat_view_exists boolean;
begin
	----------------------------------------------------------------
	select into v_mat_view_exists exists (
		select from pg_matviews
		where 1 = 1
			and pg_matviews.schemaname  = p_schema_name
			and pg_matviews.matviewname = p_mat_view_name
	);
	return v_mat_view_exists;
	----------------------------------------------------------------
end;
$$
language plpgsql;
