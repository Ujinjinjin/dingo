create or alter proc dingo.set_comment_table(
	@p_table_name  varchar(128),
	@p_comment     nvarchar(512),
	@p_schema_name varchar(128) = 'dbo'
)
as
begin
	----------------------------------------------------------------
	exec dingo.set_comment
		@p_comment     = @p_comment,
		@p_level1_name = @p_table_name,
		@p_level1_type = N'TABLE',
		@p_column_name = null,
		@p_schema_name = @p_schema_name
	;
	----------------------------------------------------------------
end;
