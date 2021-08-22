using System;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.CommandLineUtils;
using PanoramicData.ConsoleExtensions;

namespace piper.cli.Commands
{
	public class Init: CommandBase
	{
    
    private const string PiperSettingsFileName = "piper.json";
    private const int MinRevisionsToKeep = 5;
    private const int MaxRevisionsToKeep = 15;

		public Init(IConsoleOutputWriter writer, PiperSettings settings) : base(writer, settings) { }

		public void Configuration(CommandLineApplication command)
		{
			command.Description = "System initialization.";
			command.OnExecute(() =>
			{
        Writer.WriteLine("Piper initialization in progress (Welcome mate)\n\n");

        var newSettings = (PiperSettings)Settings.Clone();
        
        if (string.IsNullOrEmpty(newSettings.DbContext.HostName))
        {
          Writer.WriteLine("Database host name: ");
          newSettings.DbContext.HostName = Console.ReadLine();
        }
        else
        {
          Writer.WriteLine($"Database host name ({newSettings.DbContext.HostName}): ");
          var newHostName = Console.ReadLine();
          if (!string.IsNullOrEmpty(newHostName)) newSettings.DbContext.HostName = newHostName;
        }
        
        if (!newSettings.DbContext.PortNumber.HasValue)
        {
          Writer.WriteLine("Database port number: ");
          newSettings.DbContext.PortNumber = Convert.ToInt32(Console.ReadLine());
        }
        else
        {
          Writer.WriteLine($"Database port number ({newSettings.DbContext.PortNumber}): ");
          var newPortNumberAsString = Console.ReadLine();
          if (!string.IsNullOrEmpty(newPortNumberAsString)) newSettings.DbContext.PortNumber = Convert.ToInt32(newPortNumberAsString);
        }

        if (!newSettings.MaxRevisionsToKeep.HasValue)
        {
          Writer.WriteLine($"Max revisions to keep ({MinRevisionsToKeep} - {MaxRevisionsToKeep}): ");
          newSettings.MaxRevisionsToKeep = Convert.ToInt32(Console.ReadLine());
          if (newSettings.MaxRevisionsToKeep < MinRevisionsToKeep) newSettings.MaxRevisionsToKeep = MinRevisionsToKeep;
          if (newSettings.MaxRevisionsToKeep > MaxRevisionsToKeep) newSettings.MaxRevisionsToKeep = MaxRevisionsToKeep;
        }
        else
        {
          Writer.WriteLine($"Max revisions to keep ({newSettings.MaxRevisionsToKeep}): ");
          var newMaxRevisionsToSave = Console.ReadLine();
          if (!string.IsNullOrEmpty(newMaxRevisionsToSave)) newSettings.MaxRevisionsToKeep = Convert.ToInt32(newMaxRevisionsToSave);
          if (newSettings.MaxRevisionsToKeep < MinRevisionsToKeep) newSettings.MaxRevisionsToKeep = MinRevisionsToKeep;
          if (newSettings.MaxRevisionsToKeep > MaxRevisionsToKeep) newSettings.MaxRevisionsToKeep = MaxRevisionsToKeep;
        }

        if (string.IsNullOrEmpty(newSettings.DbContext.DatabaseName))
        {
          Writer.WriteLine("Database name: ");
          newSettings.DbContext.DatabaseName = Console.ReadLine();
        }
        else
        {
          Writer.WriteLine($"Database name ({newSettings.DbContext.DatabaseName}): ");
          var newDatabaseName = Console.ReadLine();
          if (!string.IsNullOrEmpty(newDatabaseName)) newSettings.DbContext.DatabaseName = newDatabaseName;
        }

        if (string.IsNullOrEmpty(newSettings.DbContext.Username))
        {
          Writer.WriteLine("Username: ");
          newSettings.DbContext.Username = Console.ReadLine();
        }
        else
        {
          Writer.WriteLine($"Username ({newSettings.DbContext.Username}): ");
          var newUsername = Console.ReadLine();
          if (!string.IsNullOrEmpty(newUsername)) newSettings.DbContext.Username = newUsername;
        }
        
        Writer.WriteLine("Password: ");
        newSettings.DbContext.Password = ConsolePlus.ReadPassword();
        
        File.WriteAllText(PiperSettingsFileName, JsonSerializer.Serialize(newSettings, 
          new JsonSerializerOptions
          {
            WriteIndented = true, 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
          }));
				
				return 0;
			});
		}
	}
}