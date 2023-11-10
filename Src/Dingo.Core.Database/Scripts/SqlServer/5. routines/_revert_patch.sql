create or alter procedure dingo._revert_patch(
	@p_patch_number integer
)
as
begin
	----------------------------------------------------------------
	update dingo.patch set
		reverted = 1,
		reverted_at = getutcdate()
	where patch_number = @p_patch_number;
	----------------------------------------------------------------
end;
