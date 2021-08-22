using System.IO;
using System.Linq;
using Moq;
using piper.cli;
using piper.cli.Persistence;
using Xunit;

namespace integration_tests
{
	public class WhenArgumentsPassed
	{
		private readonly Mock<IConsoleOutputWriter> _consoleMock;
		private readonly Cli _sut;

		private readonly PiperSettings _settings;
		
		public WhenArgumentsPassed()
		{
			_settings = new PiperSettings
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
			_sut = new Cli(_consoleMock.Object, _settings, new SnippetsRepository(_settings.DbContext));
		}

		[Fact]
		public void ListCommandShouldListHtmlFilesNames()
		{
			_sut.Run("list");
			_consoleMock.Verify(console=> console.WriteLine("List command excecuted successfully."), Times.Once);
		}

		[Fact]
		public void PullCommandWhitoutOptionsShouldShowPullCommandHelpMessage()
		{
			var helpText = _sut.Commands.First(c => c.Name=="pull").GetHelpText();
			_sut.Run("pull");
			_consoleMock.Verify(console=> console.WriteLine(helpText), Times.Once);
		}

		[Fact]
		public void PullAllCommandShouldShowRetrievalSuccessMessage()
		{
			_sut.Run("pull", "-a");
			_consoleMock.Verify(console=> console.WriteLine(It.Is<string>(s => s.EndsWith("snippets retrieved."))), Times.Once());
		}

		[Fact]
		public void PullKnownSnippetCommandShouldShowSnippetRetrievalSuccessMessage()
		{
			_sut.Run("pull", "-s", "_common");
			_consoleMock.Verify(console=> console.WriteLine("File _common.html retrieved successfully !"), Times.Once());
		}

		[Fact]
		public void PullUnknownSnippetsCommandShouldShowSnippetRetrievalFailMessage()
		{
			_sut.Run("pull", "-s", "my-snippet");
			_consoleMock.Verify(console=> console.WriteLine("File my-snippet.html could not be retrieved !"), Times.Once());
		}

		[Fact]
		public void PushCommandWhitotutOptionsShouldShowPushCommandHelpMessage()
		{
			var helpText = _sut.Commands.First(c => c.Name=="push").GetHelpText();
			_sut.Run("push");
			_consoleMock.Verify(console=> console.WriteLine(helpText), Times.Once);
		}

		[Fact]
		public void PushKnownMockedSnippetCommandShouldShowSnippetDeplymentSuccessMessage()
		{
			var repoMock = new Mock<ISnippetsRepository>();
			repoMock.Setup(r => r.GetMe("_common")).Returns(new Snippet 
			                                                {
				                                                Name = "_common", Html = "<div></div>"
			                                                });
			var sut = new Cli(_consoleMock.Object, _settings, repoMock.Object);
			sut.Run("push", "-s", "_common.html");
			_consoleMock.Verify(console => console.WriteLine("File _common.html pushed successfully !"));
		}
		
		[Fact]
		public void PushMissingSnippetCommandShouldShowSnippetMissingFileMessage()
		{
			_sut.Run("push", "-s", "my-snippet.html");
			_consoleMock.Verify(console=> console.WriteLine("File my-snippet.html do not exist."), Times.Once());
		}

		[Fact]
		public void PushNewSnippetCommandShouldShowMissingSnippetMessage()
		{
			File.WriteAllText("new-snippet.html", "<div></div>");
			_sut.Run("push", "-s", "new-snippet.html");
			_consoleMock.Verify(console=> console.WriteLine("There is no snippet named new-snippet, this file can not be pushed, remember to create the snippet at the portal before trying to push some content."), Times.Once());
		}

	}
	
}