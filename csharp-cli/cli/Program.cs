using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.CommandLineUtils;
using piper.cli.Persistence;

namespace piper.cli
{
  
  public class Program: CommandLineApplication
  {
    
    public static void Main(string[] args)
    {

      var writer = new ConsoleOutputWriter();
      var settings = ReadSettingsFrom("piper.json");
      
      if (settings == null)
      {
        if (args.Length > 0 && args.FirstOrDefault() == "init")
        {
          new Cli(writer, new PiperSettings(), null).Run(args);
          return;
        }

        Console.WriteLine("You must init piper in order to use it, please run piper-cli init.");
        return;
      }

      var repo = new SnippetsRepository(settings.DbContext);
      
      new Cli(writer, settings, repo).Run(args);
      
    }

    private static PiperSettings ReadSettingsFrom(string fileName)
    {
      if (!File.Exists(fileName)) return null;
      var settingsAsString = File.ReadAllText(fileName);
      return JsonSerializer.Deserialize<PiperSettings>(settingsAsString, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
    }

  }

}
