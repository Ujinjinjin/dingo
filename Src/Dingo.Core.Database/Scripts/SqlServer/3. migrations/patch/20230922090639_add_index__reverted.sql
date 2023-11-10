-- up
if dingo.exists_index('patch', 'i$patch$reverted', 'dingo') = 0
begin
	create index i$patch$reverted on dingo.patch (reverted);
end

-- down
if dingo.exists_index('patch', 'i$patch$reverted', 'dingo') = 1
begin
	exec dingo.drop_index 'patch', 'i$patch$reverted', 'dingo'
end
