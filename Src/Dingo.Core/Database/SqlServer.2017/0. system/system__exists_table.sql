create or alter function system__exists_table(
	@p_table_name  varchar(128),
	@p_schema_name varchar(128) = 'dbo'
)
returns bit
as
begin
	----------------------------------------------------------------
	declare @v_table_exists bit = 0;
	----------------------------------------------------------------
	if exists(
		select 1
		from information_schema.tables
		where 1 = 1
			and information_schema.tables.table_schema = @p_schema_name
			and information_schema.tables.table_name   = @p_table_name
	)
	begin
		set @v_table_exists = 1;
	end;
	----------------------------------------------------------------
	return @v_table_exists;
	----------------------------------------------------------------
end;
go
