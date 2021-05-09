create or alter function system__exists_constraint(
	@p_table_name      varchar(128),
	@p_constraint_name varchar(512),
	@p_schema_name     varchar(128) = 'dbo'
)
returns bit
as
begin
	----------------------------------------------------------------
	declare @v_constraint_exists bit = 0;
	----------------------------------------------------------------
	if exists(
		select 1
		from information_schema.table_constraints
		where 1 = 1
			and information_schema.table_constraints.table_schema    = @p_schema_name
			and information_schema.table_constraints.table_name      = @p_table_name
			and information_schema.table_constraints.constraint_name = @p_constraint_name
	)
	begin
		set @v_constraint_exists = 1;
	end;
	----------------------------------------------------------------
	return @v_constraint_exists;
	----------------------------------------------------------------
end;
