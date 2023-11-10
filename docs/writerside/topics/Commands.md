# commands

```Shell
dingo -h           # print usage help
dingo new          # Create new migration file
dingo up           # Apply all outdated migrations up
dingo down         # Rollback N last patches
dingo status       # Show status of migrations in working directory
dingo init         # Initialize dingo configuration profile
dingo db ping      # Ping database to check its availability
dingo logs prune   # Prune dingo log files
```

## dingo new

Creates new migrations file at given destination

```Shell
Usage:
  dingo new [options]

Options:
  -n, --name <name>  Migration name
  -p, --path <path>  Destination where migration file will be created
  -?, -h, --help     Show help and usage information
```

**Example:**

```Shell
dingo new -p ./migrations/schema/users -n create_table
```


## dingo up

Scans given root directory for outdated migrations and applies them on database using given configuration profile.

```Shell
Usage:
  dingo up [options]

Options:
  -c, --configuration <configuration>  Configuration profile name
  -p, --path <path> (REQUIRED)         Root path to database migration files
  -?, -h, --help                       Show help and usage information
```

**Examples:**

Apply migrations using `default` configuration profile:

```Shell
dingo up -p ./migrations
```

Apply migrations using `prod` configuration profile:

```Shell
dingo up -p ./migrations -c prod
```

## dingo down

Reverts given number of previously applied patches if possible. If any conflicts have emerged during scanning (e.g. migration file was modified since it was last applied), operation will stop and warn about potential risk user's carrying

> Conflicts can be ignored using `--force` flag, but be
{style="note"}

> Use `--force` option with caution, since it's a destructive operation and might lead to unrecoverable changes
{style="warning"}

```Shell
Usage:
  dingo down [options]

Options:
  -c, --configuration <configuration>  Configuration profile name
  -p, --path <path> (REQUIRED)         Root path to database migration files
  --count <count>                      Number of patches to rollback. Default: 1
  -f, --force (REQUIRED)               Ignore all warnings and rollback patches
  -?, -h, --help                       Show help and usage information
```

**Examples:**

Revert last applied patch using `default` configuration profile:

```Shell
dingo down -p ./migrations
```

Revert last **10** applied patches using `default` configuration profile:

```Shell
dingo down -p ./migrations --count 10
```

Revert last applied patch using `default` configuration profile ignoring conflicts:

```Shell
dingo down -p ./migrations -f
```

## dingo status

Scans given directory for outdated migrations against database with given configuration profile and displays their statuses in console

```Shell
Usage:
  dingo status [options]

Options:
  -c, --configuration <configuration>  Configuration profile name
  -p, --path <path> (REQUIRED)         Root path to database migration files
  -?, -h, --help                       Show help and usage information
```

**Example:**

Show status of migration files in `./migrations` using `default` configuration profile: 

```Shell
dingo status -p ./migrations
```

## dingo init

Initializes `dingo` configuration profile with given name in the given directory

```Shell
Usage:
  dingo init [options]

Options:
  -p, --path <path>                    Destination where configuration directory and files will be created. Default: current directory
  -c, --configuration <configuration>  Configuration profile name
  -?, -h, --help                       Show help and usage information
```

**Examples:**

Create `default` configuration profile at `./.dingo`

```Shell
dingo init
```

Create `prod` configuration profile at `./.dingo`

```Shell
dingo init -c prod
```
> It's recommended to initialize dingo configuration profiles at the root directory of the project and run all commands from there.
{style="note"}

## dingo db ping

Ping database using given configuration profile to check database availability

```Shell
Usage:
  dingo db ping [options]

Options:
  -c, --configuration <configuration>  Configuration profile name
  -?, -h, --help                       Show help and usage information
```

## dingo logs prune

Delete all `dingo` log files

```Shell
Usage:
  dingo logs prune [options]

Options:
  -?, -h, --help  Show help and usage information
```

> `dingo` stores log files at `~/.dingo/logs`
{style="note"}
