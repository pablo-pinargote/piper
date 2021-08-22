using System;

namespace piper.cli
{
	
	public interface IConsoleOutputWriter
	{
		void WriteLine(string s);
	}
	
	public class ConsoleOutputWriter: IConsoleOutputWriter
	{
		public void WriteLine(string s)
		{
			Console.WriteLine(s);
		}

	}
}