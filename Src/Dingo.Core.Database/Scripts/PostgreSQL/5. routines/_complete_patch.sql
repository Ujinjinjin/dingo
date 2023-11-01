-- Drop function
select dingo.drop_routine('_complete_patch', 'dingo');

-- Create function
create function dingo._complete_patch(
	p_patch_number integer
)
returns void as
$$
begin
	----------------------------------------------------------------
	update dingo.patch set
		applied_at = timezone('utc', now())
	where 1 = 1
		and patch_number = p_patch_number;
	----------------------------------------------------------------
end;
$$
language plpgsql;
