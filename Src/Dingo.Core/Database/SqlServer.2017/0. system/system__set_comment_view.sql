create or alter proc system__set_comment_view(
	@p_view_name   varchar(128),
	@p_comment     nvarchar(512),
	@p_schema_name varchar(128) = 'dbo'
)
as
begin
	----------------------------------------------------------------
	exec system__set_comment
		@p_comment     = @p_comment,
		@p_level1_name = @p_view_name,
		@p_level1_type = N'VIEW',
		@p_column_name = null,
		@p_schema_name = @p_schema_name
	;
	----------------------------------------------------------------
end;
go
