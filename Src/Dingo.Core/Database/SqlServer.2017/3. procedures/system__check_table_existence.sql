create or alter procedure system__check_table_existence(
	@p_table_schema varchar(128),
	@p_table_name varchar(128)
)
as
begin
	----------------------------------------------------------------
	declare @v_table_exists bit = 0;
	----------------------------------------------------------------
	if object_id(@p_table_name) is not null
		set @v_table_exists = 1;
	----------------------------------------------------------------
	select @v_table_exists as system__check_table_existence;
	----------------------------------------------------------------
end;
