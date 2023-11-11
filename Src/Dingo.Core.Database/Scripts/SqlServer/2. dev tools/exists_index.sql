create or alter function dingo.exists_index(
	@p_table_name  varchar(128),
	@p_index_name  varchar(512),
	@p_schema_name varchar(128) = 'dbo'
)
returns bit
as
begin
	----------------------------------------------------------------
	declare @v_index_exists bit = 0;
	----------------------------------------------------------------
	if exists(
		select 1
		from sys.indexes
		inner join information_schema.tables
			on information_schema.tables.table_schema           = @p_schema_name
			and object_id(information_schema.tables.table_name) = sys.indexes.object_id
			and information_schema.tables.table_name            = @p_table_name
		where 1 = 1
			and sys.indexes.name = @p_index_name
	)
	begin
		set @v_index_exists = 1;
	end;
	----------------------------------------------------------------
	return @v_index_exists;
	----------------------------------------------------------------
end;
