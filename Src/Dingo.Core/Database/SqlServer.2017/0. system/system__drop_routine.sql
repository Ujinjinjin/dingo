create or alter procedure system__drop_routine(
	@p_routine_name varchar(max),
	@p_schema_name  varchar(128) = 'dbo'
)
as
begin
	----------------------------------------------------------------
	declare @c_routine_type_func varchar(16) = 'FUNCTION';
	declare @c_routine_type_proc varchar(16) = 'PROCEDURE';
	declare @v_dropped_count     int = 0;
	declare @v_row_number        int;
	declare @v_schema_name       varchar(128);
	declare @v_routine_name      varchar(max);
	declare @v_routine_type      varchar(16);
	declare @v_dynamic_sql       varchar(max);
	----------------------------------------------------------------
	select
		row_number() over(order by information_schema.routines.routine_name) as rn,
		information_schema.routines.routine_schema as schema_name,
		information_schema.routines.routine_name,
		information_schema.routines.routine_type
	into #tt_rows
	from information_schema.routines
	where 1 = 1
		and information_schema.routines.routine_schema = @p_schema_name
		and information_schema.routines.routine_name   = @p_routine_name
	;
	----------------------------------------------------------------
	select @v_row_number = min(rn) from #tt_rows;
	----------------------------------------------------------------
	while @v_row_number is not null
	begin
		----------------------------------------------------------------
		set @v_schema_name  = (select schema_name  from #tt_rows where rn = @v_row_number);
		set @v_routine_name = (select routine_name from #tt_rows where rn = @v_row_number);
		set @v_routine_type = (select routine_type from #tt_rows where rn = @v_row_number);
		----------------------------------------------------------------
		select @v_dynamic_sql =
			case
				when @v_routine_type = @c_routine_type_func then 'drop function '  + @v_schema_name + '.' + @v_routine_name + ';'
				when @v_routine_type = @c_routine_type_proc then 'drop procedure ' + @v_schema_name + '.' + @v_routine_name + ';'
			end
		;
		----------------------------------------------------------------
		execute(@v_dynamic_sql);
		set @v_dropped_count = @v_dropped_count + 1;
		----------------------------------------------------------------
		select @v_row_number = min(rn) from #tt_rows where rn > @v_row_number
		----------------------------------------------------------------
	end;
	----------------------------------------------------------------
	print 'dropped ' + cast(@v_dropped_count as varchar(10)) + ' routines';
	----------------------------------------------------------------
	----------------------------------------------------------------
end;
go
