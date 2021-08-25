# Piper

\
django CMS Snippets synchronization tool that allows us to use any tool to work with the snippets content.

## piper-cli

\
Basically it is a command line interface that allows the developer to gain access to the CMS Snippets plugins directly from the database; so he can then edit them on a high level programming IDE like PyCharm, Visual Studio Code, etc.

The tool is multiplatform (linux, windows and macOs), and it is developed using .net core and c#.

We can download the app from the release section of this repo or we can clone the repo and build it as we needed.

In order to use the tool we recommend to follow this steps:

1. Create a folder called "snippets" under the djangoCMS application root.
2. Copy the executable "piper-cli" into the "snippets" folder or add the cli to the global bin of your operating system.

Right after we "install" or copy the application to our platform we can start using it by issuing the command:

```bash
piper-cli
```

3. Initialize the environment by running the "init" command.

The first time we run the tool it will ask for initialization, which is possible by using the "init" command:

```bash
piper-cli init
```

Once we execute this command we will be ask for the following information:

a. Database host name
b. Database port number
c. Max revisions to save
d. Database name
e. Database username
f. Database password

This parameters are all mandatory and will be stored in a local file right where we are using the tool (Working directory); the file will have the following content:

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

- MySql.Data (At the moment it only works with django CMS running over a MySql database)
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

#### Pulling all the Snippets

To pull all the snippets at once we can run the following command:

```bash
piper-cli pull -a
```

#### Pulling especific Snippets

To pull one or more particular snippets we can run the pull command with the -s option, like this:

```bash
piper-cli pull -s snippet-1 -s snippet-2 -s snippet-3
```

#### Pushin Snippets

To push one or more snippets to the database we can run the push command with the -s option, like this:

```bash
piper-cli push -s snippet-1.html
```
