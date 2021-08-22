using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;
using piper.cli.Persistence;

namespace piper.cli.Commands
{
	public class PullSnippets: CommandBase
	{
		public PullSnippets(IConsoleOutputWriter writer, PiperSettings settings, ISnippetsRepository snippetsRepository): base(writer, settings, snippetsRepository) { }

		public void Configuration(CommandLineApplication command)
		{
			command.Description = "Pull snippets as html files.";
			command.HelpOption("-?|-h|--help");
			var allSnippets = command.Option("-a | --all", "All snippets", CommandOptionType.NoValue);
			var snippets = command.Option("-s | --snippets <SNIPPET_1> <SNIPPET_2> <SNIPPET_N> ", "Snippets names list", CommandOptionType.MultipleValue);
			command.OnExecute(() =>
			{

				if (!allSnippets.HasValue() && !snippets.HasValue())
				{
					Writer.WriteLine(command.GetHelpText());
					return 0;
				}

				if (snippets.Values.Count > 0)
				{
					var names = snippets.Values.ToList();
					var getByNamesResult = new List<Snippet>(SnippetsRepository.GetByNames(names));
					var snippetsRetreivedCounter = 0;
					foreach (var name in snippets.Values)
					{
						var snippet = getByNamesResult.FirstOrDefault(r => r.Name == name);
						if (snippet == null)
						{
							Writer.WriteLine($"File {name}.html could not be retrieved !");
						}
						else
						{
							snippetsRetreivedCounter++;
							File.WriteAllText($"{snippet.Name}.html", snippet.Html);
							Writer.WriteLine($"File {snippet.Name}.html retrieved successfully !");
						}
					}
					Writer.WriteLine($"{snippetsRetreivedCounter} snippets retrieved.");
				}
				else
				{
					var getAllResult = new List<Snippet>(SnippetsRepository.GetAll());
					foreach (var snippet in getAllResult)
					{
						File.WriteAllText($"{snippet.Name}.html", snippet.Html);
						Writer.WriteLine($"File {snippet.Name}.html retrieved successfully !");
					}
					Writer.WriteLine($"{getAllResult.Count} snippets retrieved.");
				}

				return 0;
				
			});
		}
	}
}