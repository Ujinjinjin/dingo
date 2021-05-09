create or alter procedure system__drop_default(
	@p_table_name  varchar(128),
	@p_column_name varchar(128),
	@p_schema_name varchar(128) = 'dbo'
)
as
begin
	----------------------------------------------------------------
	declare @c_def_constraint_type char = 'D';
	declare @v_dropped_count       int = 0;
	declare @v_row_number          int;
	declare @v_constraint_name     varchar(512);
	----------------------------------------------------------------
	if dbo.system__exists_column(@p_table_name, @p_column_name, @p_schema_name) = 1
	begin
		----------------------------------------------------------------
		select
			row_number() over(order by name) as rn,
			sys.default_constraints.name as constraint_name
		into #tt_rows
		from sys.default_constraints
		where 1 = 1
			and sys.default_constraints.parent_object_id = object_id(@p_table_name)
			and sys.default_constraints.type             = @c_def_constraint_type
			and sys.default_constraints.parent_column_id = (
				select columns.column_id
				from sys.columns
				where 1 = 1
					and columns.object_id = object_id(@p_table_name)
					and columns.name      = @p_column_name
			)
		;
		----------------------------------------------------------------
		select @v_row_number = min(rn) from #tt_rows;
		----------------------------------------------------------------
		while @v_row_number is not null
		begin
			----------------------------------------------------------------
			set @v_constraint_name = (select constraint_name from #tt_rows where rn = @v_row_number);
			----------------------------------------------------------------
			execute('alter table ' + @p_schema_name + '.' + @p_table_name + ' drop constraint if exists ' + @v_constraint_name + ';');
			set @v_dropped_count = @v_dropped_count + 1;
			----------------------------------------------------------------
			select @v_row_number = min(rn) from #tt_rows where rn > @v_row_number;
			----------------------------------------------------------------
		end;
		----------------------------------------------------------------
	end;
	----------------------------------------------------------------
	print 'dropped ' + cast(@v_dropped_count as varchar(10)) + ' default constraints';
	----------------------------------------------------------------
end;
