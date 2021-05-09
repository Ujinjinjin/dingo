create or alter procedure system__drop_default(
	@p_table_name  varchar(128),
	@p_index_name  varchar(512),
	@p_schema_name varchar(128) = 'dbo'
)
as
begin
	----------------------------------------------------------------
	declare @v_dropped_count int = 0;
	declare @v_row_number    int;
	declare @v_table_schema  varchar(128);
	declare @v_table_name    varchar(128);
	declare @v_index_name    varchar(512);
	----------------------------------------------------------------
	if dbo.system__exists_index(@p_table_name, @p_index_name, @p_schema_name) = 1
	begin
		----------------------------------------------------------------
		select
			row_number() over(order by sys.indexes.name) as rn,
			information_schema.tables.table_schema as schema_name,
			information_schema.tables.table_name,
			sys.indexes.name as index_name
		into #tt_rows
		from sys.indexes
		inner join information_schema.tables
			on information_schema.tables.table_schema           = @p_schema_name
			and object_id(information_schema.tables.table_name) = sys.indexes.object_id
			and information_schema.tables.table_name            = @p_table_name
		where 1 = 1
			and sys.indexes.name = @p_index_name
		;
		----------------------------------------------------------------
		select @v_row_number = min(rn) from #tt_rows;
		----------------------------------------------------------------
		while @v_row_number is not null
		begin
			----------------------------------------------------------------
			set @v_table_schema = (select schema_name from #tt_rows where rn = @v_row_number);
			set @v_table_name   = (select table_name  from #tt_rows where rn = @v_row_number);
			set @v_index_name   = (select index_name  from #tt_rows where rn = @v_row_number);
			----------------------------------------------------------------
			execute('drop index ' + @p_schema_name + '.' + @p_table_name + '.' + @v_index_name + ';');
			set @v_dropped_count = @v_dropped_count + 1;
			----------------------------------------------------------------
			select @v_row_number = min(rn) from #tt_rows where rn > @v_row_number;
			----------------------------------------------------------------
		end;
		----------------------------------------------------------------
	end;
	----------------------------------------------------------------
	print 'dropped ' + cast(@v_dropped_count as varchar(10)) + ' default indices';
	----------------------------------------------------------------
end;
