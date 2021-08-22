using System.Linq;
using Moq;
using piper.cli;
using piper.cli.Persistence;
using Xunit;

namespace integration_tests
{
	public class WhenNoArgumentsPassed
	{

		private readonly Mock<IConsoleOutputWriter> _consoleMock;
		private readonly Cli _sut;
		
		public WhenNoArgumentsPassed()
		{
			var settings = new PiperSettings
			               {
				               DbContext = new MySqlDbContext
				                           {
					                           HostName = "localhost",
					                           DatabaseName = "paranoid_portal",
					                           Username = "root",
					                           PortNumber = 3306
				                           },
				               MaxRevisionsToKeep = 5
			               };
			_consoleMock = new Mock<IConsoleOutputWriter>();
			_sut = new Cli(_consoleMock.Object, settings, new SnippetsRepository(settings.DbContext));
		}
	
		[Fact]
		public void ShouldShowHelpMessage()
		{
			var helpText = _sut.GetHelpText();
			_sut.Run();
			_consoleMock.Verify(console=> console.WriteLine(helpText), Times.Once);
		}

		[Fact]
		public void HelplMessageShouldListAllSupportedCommands()
		{
			var commands = new []
			               {
				               "init",
				               "list",
				               "pull",
				               "push"
			               };
			_sut.Run();
			_consoleMock.Verify(console=> console.WriteLine(It.Is<string>(s=> commands.All(s.Contains))), Times.Once);
		}

	}
}