create or alter proc system__set_comment_table_column(
	@p_table_name  varchar(128),
	@p_column_name varchar(128),
	@p_comment     nvarchar(512),
	@p_schema_name varchar(128) = 'dbo'
)
as
begin
	----------------------------------------------------------------
	exec system__set_comment
		@p_comment     = @p_comment,
		@p_level1_name = @p_table_name,
		@p_level1_type = N'TABLE',
		@p_column_name = @p_column_name,
		@p_schema_name = @p_schema_name
	;
	----------------------------------------------------------------
end;
go
