create or alter function dingo.exists_type(
	@p_type_name   varchar(512),
	@p_schema_name varchar(128) = 'dbo'
)
returns bit
as
begin
	----------------------------------------------------------------
	declare @v_type_exists bit = 0;
	----------------------------------------------------------------
	if exists(
		select 1
		from sys.types
		where 1 = 1
			and sys.types.schema_id = schema_id(@p_schema_name)
			and sys.types.name      = @p_type_name
	)
	begin
		set @v_type_exists = 1;
	end;
	----------------------------------------------------------------
	return @v_type_exists;
	----------------------------------------------------------------
end;
go
