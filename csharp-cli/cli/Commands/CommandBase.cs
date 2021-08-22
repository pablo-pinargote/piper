using piper.cli.Persistence;

namespace piper.cli.Commands
{
	public class CommandBase
	{
		protected readonly IConsoleOutputWriter Writer;
		protected readonly ISnippetsRepository SnippetsRepository;
		protected readonly PiperSettings Settings;

		protected CommandBase(IConsoleOutputWriter writer, PiperSettings settings)
		{
			Writer = writer;
			Settings = settings;
		}
		
		protected CommandBase(IConsoleOutputWriter writer, 
		                      PiperSettings settings, 
		                      ISnippetsRepository snippetsRepository)
		{
			Writer = writer;
			Settings = settings;
			SnippetsRepository = snippetsRepository;
		}
	}
}