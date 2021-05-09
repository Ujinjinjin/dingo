create or alter function system__exists_column(
	@p_table_name  varchar(128),
	@p_column_name varchar(128),
	@p_schema_name varchar(128) = 'dbo'
)
returns bit
as
begin
	----------------------------------------------------------------
	declare @v_column_exists bit = 0;
	----------------------------------------------------------------
	if exists(
		select 1
		from information_schema.columns
		where 1 = 1
			and information_schema.columns.table_schema = @p_schema_name
			and information_schema.columns.table_name   = @p_table_name
			and information_schema.columns.column_name  = @p_column_name
	)
	begin
		set @v_column_exists = 1;
	end;
	----------------------------------------------------------------
	return @v_column_exists;
	----------------------------------------------------------------
end;
