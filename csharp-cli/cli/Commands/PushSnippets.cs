using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;
using piper.cli.Persistence;

namespace piper.cli.Commands
{
	public class PushSnippets : CommandBase
	{
		private const string PiperRevisionsFolderName = ".piper";
		public PushSnippets(IConsoleOutputWriter writer, PiperSettings settings, ISnippetsRepository snippetsRepository): base(writer, settings, snippetsRepository) { }

		public void Configuration(CommandLineApplication command)
		{
			command.Description = "Push snippets to the configured database.";
			command.HelpOption("-?|-h|--help");
			var snippets = command.Option("-s | --snippets <SNIPPET_1> <SNIPPET_2> <SNIPPET_N> ", "Snippets names list", CommandOptionType.MultipleValue);
			command.OnExecute(() =>
			{
				if (!snippets.HasValue())
				{
					Writer.WriteLine(command.GetHelpText());
					return 0;
				}

				var snippetsPushedCounter = 0;
				foreach (var fileNameWithExtension in snippets.Values)
				{
					if (!File.Exists(fileNameWithExtension))
					{
						Writer.WriteLine($"File {fileNameWithExtension} do not exist.");
						continue;
					}

					var htmlContent = File.ReadAllText(fileNameWithExtension);
					var snippetName = fileNameWithExtension.Split('.').First();
					var snippet = SnippetsRepository.GetMe(snippetName);
					
					if (snippet == null || string.IsNullOrEmpty(snippet.Html))
					{
						Writer.WriteLine($"There is no snippet named {snippetName}, this file can not be pushed, remember to create the snippet at the portal before trying to push some content.");
						continue;
					}

					if (snippet.Html == htmlContent)
					{
						Writer.WriteLine($"No changes detected in {fileNameWithExtension}, so it will not be pushed.");
						continue;
					}
					
					if (!Directory.Exists(PiperRevisionsFolderName)) Directory.CreateDirectory(PiperRevisionsFolderName);
					File.WriteAllText($"{PiperRevisionsFolderName}/{snippetName}.{DateTime.Now.Ticks}.html", snippet.Html);
					var info = new DirectoryInfo(PiperRevisionsFolderName);
					var files = info.GetFiles($"{snippetName}.*").OrderBy(p => p.CreationTime).ToArray();
					while (files.Length > Settings.MaxRevisionsToKeep)
					{
						files.First().Delete();
						files = info.GetFiles($"{snippetName}.*").OrderBy(p => p.CreationTime).ToArray();
					}
					SnippetsRepository.UpdateHtmlContent(snippetName, htmlContent);
					Writer.WriteLine($"File {fileNameWithExtension} pushed successfully !");
					snippetsPushedCounter++;
				}
				
				Writer.WriteLine($"{snippetsPushedCounter} snippets pushed successfully.");
				return 0;
				
			});
		}
	}
}