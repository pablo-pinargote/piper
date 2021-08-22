# Piper

\
django CMS Snippets synchronization tool that allows us to use any IDE to edit the content of every snippet hosted on our django CMS based portals.

## piper-cli

\
The tool is a multiplatform (linux, windows and macOs) CLI developed using .net core and c#.

We can download the app from the release section of this repo or we can clone the repo and build it as we needed.

Right after we "install" or copy the application for our platform we can start using it by issuion the command:

```bash
piper-cli
```

The firsta time we run the tool it will ask for initialization, which is possible by using the "init" command:

```bash
piper-cli init
```

The initialization parameters are all mandatory and will be stored in a local file right where we invoke the tool, and it will have the following content:

```json
{
    "dbContext": {
        "hostName": "my-host",
        "portNumber": 12345,
        "username": "my-user-name",
        "password": "my-passwd",
        "databaseName": "portal_db_name"
    },
    "maxRevisionsToKeep": 10
}
```

### Dependencies

\
The tool depends on .net core framework and uses the following libraries in order to work:

- MySql.Data (It only works with django CMS running over a MySql database)
- Microsoft.Extensions.CommandLineUtils
- PanoramicData.ConsoleExtensions 

### Available commands

\
The tool support 3 more commands, which are self explanatory: list, pull and push.

Using this commands we can query or **list** the available snippets, **pull** whatever snippet we need to work on and finally when the work is done we can **push** the changes back to the database.

To see the CLI in action after the initialization process is completed we just run the tool one more time:

```bash
piper-cli

Usage: piper-cli [options] [command]

Options:
  -?|-h|--help  Show help information

Commands:
  init  System initialization.
  list  List all snippets.
  pull  Pull snippets as html files.
  push  Push snippets to the configured database.

Use "piper-cli [command] --help" for more information about a command.
```
