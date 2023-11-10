create or alter procedure dingo._complete_patch(
	@p_patch_number integer
)
as
begin
	----------------------------------------------------------------
	update dingo.patch set
		applied_at = getutcdate()
	where patch_number = @p_patch_number;
	----------------------------------------------------------------
end;
