using System.Linq;
using Microsoft.Extensions.CommandLineUtils;
using piper.cli.Persistence;

namespace piper.cli.Commands
{
	public class ListSnippets: CommandBase
	{
		public ListSnippets(IConsoleOutputWriter writer, 
		                    PiperSettings settings, 
		                    ISnippetsRepository snippetsRepository): base(writer, settings, snippetsRepository) { }
		
		public void Configuration(CommandLineApplication command)
		{
			command.Description = "List all snippets.";
			command.OnExecute(() =>
			{
				var snippetsNames = SnippetsRepository.GetAllNames().ToArray();
				Writer.WriteLine($"Listing {snippetsNames.Length} snippets.");
				foreach (var item in snippetsNames)
				{
					Writer.WriteLine($"{item}.html");
				}
				Writer.WriteLine("List command excecuted successfully.");
				return 0;
			});
		}
	}
}