create or alter procedure system__drop_constraint(
	@p_table_name      varchar(128),
	@p_constraint_name varchar(512),
	@p_schema_name     varchar(128) = 'dbo'
)
as
begin
	----------------------------------------------------------------
	declare @v_dropped_count   int = 0;
	declare @v_row_number      int;
	declare @v_schema_name     varchar(128);
	declare @v_table_name      varchar(128);
	declare @v_constraint_name varchar(512);
	----------------------------------------------------------------
	if dbo.system__exists_constraint(@p_table_name, @p_constraint_name, @p_schema_name) = 1
	begin
		----------------------------------------------------------------
		select
			row_number() over(order by table_schema, table_name, constraint_name) as rn,
			information_schema.table_constraints.table_schema as schema_name,
			information_schema.table_constraints.table_name,
			information_schema.table_constraints.constraint_name
		into #tt_rows
		from information_schema.table_constraints
		where 1 = 1
			and information_schema.table_constraints.table_schema    = @p_schema_name
			and information_schema.table_constraints.table_name      = @p_table_name
			and information_schema.table_constraints.constraint_name = @p_constraint_name
		;
		----------------------------------------------------------------
		select @v_row_number = min(rn) from #tt_rows;
		----------------------------------------------------------------
		while @v_row_number is not null
		begin
			----------------------------------------------------------------
			set @v_schema_name     = (select schema_name     from #tt_rows where rn = @v_row_number);
			set @v_table_name      = (select table_name      from #tt_rows where rn = @v_row_number);
			set @v_constraint_name = (select constraint_name from #tt_rows where rn = @v_row_number);
			----------------------------------------------------------------
			execute('alter table ' + @v_schema_name + '.' + @v_table_name + ' drop constraint [' + @v_constraint_name + ']');
			set @v_dropped_count = @v_dropped_count + 1;
			----------------------------------------------------------------
			select @v_row_number = min(rn) from #tt_rows where rn > @v_row_number;
			----------------------------------------------------------------
		end;
		----------------------------------------------------------------
	end;
	----------------------------------------------------------------
	print 'dropped ' + cast(@v_dropped_count as varchar(10)) + ' constraints';
	----------------------------------------------------------------
end;
