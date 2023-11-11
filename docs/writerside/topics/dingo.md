# dingo

`dingo` is a framework-agnostic lightweight cross-platform database migration tool that will keep your database schema in sync across multiple developers and deployment environments.

It's a command line tool that can be used on any project regardless of the programming language used to create it, be it Python, Go, C#, Java, Ruby, PHP or something else. This tool fits especially well with microservice architecture and services written in different languages, allowing you to have consistent and unified database migration process.

## Key Features

- Supports Postgres and SQL Server
- Migrations are timestamp-versioned
- Migrations are written using plain `.sql`
- Project configs can be stored as `.yml`, `.json` or environment variables
- Use different configuration profiles for different environments
- Provides tools for rapid database development
- Apply and revert database migrations

## Installation

Currently, `dingo` can only be installed as a binary using default package management tools on Linux and macOS. If you'd like to make dingo a part of `homebrew`, `apt` or any other index, feel free to contribute, any help would be highly appreciated.

### Install

<tabs group="platform">
<tab title="Linux" group-key="platform-linux">
    <p>First, download and install the binary:</p>
    <code-block lang="shell">
        curl -s https://api.github.com/repos/ujinjinjin/dingo/releases/latest \
            | grep "browser_download_url.*deb" \
            | cut -d '"' -f 4 \
            | wget -O dingo.deb -qi -
        sudo dpkg --install dingo.deb
    </code-block>
    <p>Then add <code>/usr/share/dingo</code> to the <code>$PATH</code> environment variable.</p>
    <p>Restart your terminal shell and run following command to validate successful installation:</p>
    <code-block lang="shell">dingo --version</code-block>
</tab>
<tab title="macOS" group-key="platform-macOS">
    <p>Download and install the binary:</p>
    <code-block lang="shell">
        curl -s https://api.github.com/repos/ujinjinjin/dingo/releases/latest \
            | grep "browser_download_url.*pkg" \
            | cut -d '"' -f 4 \
            | xargs -I packageUrl curl -s -o dingo.pkg packageUrl
        installer -pkg dingo.pkg -target CurrentUserHomeDirectory
    </code-block>
</tab>
</tabs>

### Uninstall

<tabs group="platform">
<tab title="Linux" group-key="platform-linux">
    <p>To uninstall <code>dingo</code> use:</p>
    <code-block lang="shell">sudo dpkg --remove dingo</code-block>
</tab>
<tab title="macOS" group-key="platform-macOS">
    <p>To uninstall <code>dingo</code> use:</p>
    <code-block lang="shell">
        rm -rfd ~/.dingo /usr/local/bin/dingo
    </code-block>
</tab>
</tabs>

## Basic usage

### Create migration

```Shell
dingo new -n create_table -p ./migrations/schemas
```

The command above will create `20231105214436_create_table.sql` at `.../migrations/schemas`.

### Apply

```Shell
dingo up -p ./migrations -c prod
```

The command above will apply migrations located at `.../migrations` using `prod` configuration profile.

For more detailed info on usage refer to [wiki section](Commands.md) or run:

```Shell
dingo -h
```

## Contribute

Any contributions are welcome
