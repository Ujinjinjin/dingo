create table dingo_script(
	dingo_script_id integer,
	script_path text,
	script_hash char(256),
	date_updated date,
	primary key (dingo_script_id)
);