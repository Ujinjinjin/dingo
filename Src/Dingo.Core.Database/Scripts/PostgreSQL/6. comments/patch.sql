-- Set comments
comment on table dingo.patch is 'database patch';
comment on column dingo.patch.patch_number is 'patch number';
comment on column dingo.patch.applied_at is 'timestamp when patch was applied';
comment on column dingo.patch."type" is 'patch type';
comment on column dingo.patch.reverted_at is 'timestamp when patch was reverted';
comment on column dingo.patch.reverted is 'indicates that patch was reverted';
