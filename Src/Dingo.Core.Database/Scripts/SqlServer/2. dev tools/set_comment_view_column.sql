create or alter proc dingo.set_comment_view_column(
	@p_view_name   varchar(128),
	@p_column_name varchar(128),
	@p_comment     nvarchar(512),
	@p_schema_name varchar(128) = 'dbo'
)
as
begin
	----------------------------------------------------------------
	exec dingo.set_comment
		@p_comment     = @p_comment,
		@p_level1_name = @p_view_name,
		@p_level1_type = N'VIEW',
		@p_column_name = @p_column_name,
		@p_schema_name = @p_schema_name
	;
	----------------------------------------------------------------
end;
