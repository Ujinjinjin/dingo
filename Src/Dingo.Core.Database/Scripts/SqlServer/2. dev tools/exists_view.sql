create or alter function dingo.exists_view(
	@p_view_name   varchar(128),
	@p_schema_name varchar(128) = 'dbo'
)
returns bit
as
begin
	----------------------------------------------------------------
	declare @v_view_exists bit = 0;
	----------------------------------------------------------------
	if exists(
		select 1
		from sys.views
		where 1 = 1
			and sys.views.schema_id = schema_id(@p_schema_name)
			and sys.views.name      = @p_view_name
	)
	begin
		set @v_view_exists = 1;
	end;
	----------------------------------------------------------------
	return @v_view_exists;
	----------------------------------------------------------------
end;
