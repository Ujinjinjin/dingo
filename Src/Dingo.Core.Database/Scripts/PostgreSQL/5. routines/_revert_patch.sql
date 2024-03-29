-- Drop function
select dingo.drop_routine('_revert_patch', 'dingo');

-- Create function
create function dingo._revert_patch(
	p_patch_number integer
)
returns void as
$$
begin
	----------------------------------------------------------------
	update dingo.patch set
		reverted = true,
		reverted_at = timezone('utc', now())
	where patch_number = p_patch_number;
	----------------------------------------------------------------
end;
$$
language plpgsql;
