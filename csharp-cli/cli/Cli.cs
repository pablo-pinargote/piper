using System;
using Microsoft.Extensions.CommandLineUtils;
using piper.cli.Commands;
using piper.cli.Persistence;

namespace piper.cli
{
	public class Cli: CommandLineApplication
	{
	
		public Cli(IConsoleOutputWriter writer, PiperSettings settings, ISnippetsRepository repo)
		{
			
			Name = "piper-cli";
			Description = "djangocms snippets synchronization tool.";

			HelpOption("-?|-h|--help");
			
			Command("init", new Init(writer, settings).Configuration);
			Command("list", new ListSnippets(writer, settings, repo).Configuration);
			Command("pull", new PullSnippets(writer, settings, repo).Configuration);
			Command("push", new PushSnippets(writer, settings, repo).Configuration);

			OnExecute(() =>
			{
				writer.WriteLine(GetHelpText());
				return 0;
			});

		}

		public int Run(params string[] args)
		{
			int result;
			try
			{
				result = Execute(args);
			}
			catch (Exception)
			{
				Console.WriteLine("Something went wrong, check the settings stored at your piper.json file.");
				// TODO: Activate simple logging mechanism using files to log unhandled exceptions
				ShowHelp();
				result = -1;
			}
			return result;
		}

	}
}