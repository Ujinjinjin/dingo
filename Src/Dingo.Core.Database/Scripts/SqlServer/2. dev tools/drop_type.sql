create or alter procedure dingo.drop_type(
	@p_type_name   varchar(512),
	@p_schema_name varchar(128) = 'dbo'
)
as
begin
	----------------------------------------------------------------
	declare @c_class_desc        varchar(4) = 'TYPE';
	declare @v_dropped_count     int = 0;
	declare @v_dropped_dep_count int = 0;
	declare @v_row_number        int;
	declare @v_type_name         varchar(512);
	declare @v_dependency_type   varchar(128);
	declare @v_dependency_name   varchar(128);
	declare @v_schema_name       varchar(128);
	declare @v_dynamic_sql       varchar(max);
	----------------------------------------------------------------
	if dingo.exists_type(@p_type_name, @p_schema_name) = 1
	begin
		----------------------------------------------------------------
		select distinct
			row_number() over (order by sys.objects.type) as rn,
			sys.objects.type as dependency_type,
			quotename(object_schema_name(sql_expression_dependencies.referencing_id)) as schema_name,
			quotename(object_name(sql_expression_dependencies.referencing_id)) as dependency_name
		into #tt_dependencies
		from sys.sql_expression_dependencies
		inner join sys.objects
			on sys.objects.object_id  = sql_expression_dependencies.referencing_id
			and sys.objects.schema_id = schema_id(@p_schema_name)
		where 1 = 1
			and sql_expression_dependencies.referenced_class_desc  = @c_class_desc
			and sql_expression_dependencies.referenced_entity_name = @p_type_name
		;
		----------------------------------------------------------------
		select @v_row_number = min(rn) from #tt_dependencies;
		----------------------------------------------------------------
		while @v_row_number is not null
		begin
			----------------------------------------------------------------
			set @v_dependency_type = (select dependency_type from #tt_dependencies where rn = @v_row_number);
			set @v_dependency_name = (select dependency_name from #tt_dependencies where rn = @v_row_number);
			set @v_schema_name     = (select schema_name     from #tt_dependencies where rn = @v_row_number);
			----------------------------------------------------------------
			select @v_dynamic_sql =
				case
					when @v_dependency_type in('P')
						then 'drop procedure ' + @v_schema_name + '.' + @v_dependency_name
					when @v_dependency_type in('TF', 'IF', 'F')
						then 'drop function '  + @v_schema_name + '.' + @v_dependency_name
				end
			;
			----------------------------------------------------------------
			if @v_dynamic_sql is not null
			begin
				execute(@v_dynamic_sql);
				set @v_dropped_dep_count = @v_dropped_dep_count + 1;
			end;
			----------------------------------------------------------------
			select @v_row_number = min(rn) from #tt_dependencies where rn > @v_row_number;
			----------------------------------------------------------------
		end;
		----------------------------------------------------------------
		select
			row_number() over (order by sys.types.name) as rn,
			sys.types.name as type_name
		into #tt_rows
		from sys.types
		where 1 = 1
			and sys.types.schema_id = schema_id(@p_schema_name)
			and sys.types.name      = @p_type_name
		;
		----------------------------------------------------------------
		select @v_row_number = min(rn) from #tt_rows;
		----------------------------------------------------------------
		while @v_row_number is not null
		begin
			----------------------------------------------------------------
			set @v_type_name = (select type_name from #tt_rows where rn = @v_row_number);
			----------------------------------------------------------------
			execute('drop type ' + @p_schema_name + '.' + @v_type_name + ';');
			set @v_dropped_count = @v_dropped_count + 1;
			----------------------------------------------------------------
			select @v_row_number = min(rn) from #tt_rows where rn > @v_row_number;
			----------------------------------------------------------------
		end;
		----------------------------------------------------------------
	end;
	----------------------------------------------------------------
	print 'dropped ' + cast(@v_dropped_dep_count as varchar(10)) + ' dependencies';
	print 'dropped ' + cast(@v_dropped_count as varchar(10)) + ' types';
	----------------------------------------------------------------
end;
go
