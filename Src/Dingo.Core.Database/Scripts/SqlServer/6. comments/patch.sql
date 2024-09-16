-- Set comments
exec dingo.set_comment_table 'patch', 'database patch', 'dingo';
exec dingo.set_comment_table_column 'patch', 'patch_number', 'patch number', 'dingo';
exec dingo.set_comment_table_column 'patch', 'applied_at', 'timestamp when patch was applied', 'dingo';
exec dingo.set_comment_table_column 'patch', 'type', 'patch type', 'dingo';
exec dingo.set_comment_table_column 'patch', 'reverted_at', 'timestamp when patch was reverted', 'dingo';
exec dingo.set_comment_table_column 'patch', 'reverted', 'indicates that patch was reverted', 'dingo';
