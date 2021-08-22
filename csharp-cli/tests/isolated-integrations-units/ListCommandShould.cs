using System.Linq;
using Moq;
using piper.cli;
using piper.cli.Persistence;
using Xunit;

namespace tests.isolated_integrations_units
{

	public class ListCommandShould
	{
		private readonly Mock<IConsoleOutputWriter> _consoleMock;
		private readonly SnippetsRepository _repo;
		private readonly Cli _sut;

		public ListCommandShould()
		{
			_consoleMock = new Mock<IConsoleOutputWriter>();
			var settings = new PiperSettings
			               {
				               DbContext = new MySqlDbContext
				                           {
					                           HostName = "127.0.0.1",
					                           PortNumber = 32574,
					                           Username = "root",
					                           Password = "password",
					                           DatabaseName = "quickstart_portal"
				                           },
				               MaxRevisionsToKeep = 5
			               };
			_repo = new SnippetsRepository(settings.DbContext);
			_sut = new Cli(_consoleMock.Object, settings, _repo);
		}
		
		[Fact]
		public void ShowListingSnippetsMessage()
		{
			_sut.Run("list");
			_consoleMock.Verify(console => console.WriteLine($"Listing {_repo.GetAllNames().Count()} snippets."), Times.Once);
		}
		
		[Fact]
		public void ListSnippetsFileNames()
		{
			_sut.Run("list");
			_consoleMock.Verify(console => console.WriteLine(It.Is<string>(s=>s.EndsWith(".html"))), Times.Exactly(_repo.GetAllNames().Count()));
		}

		[Fact]
		public void ShowListCommandSuccesMessage()
		{
			_sut.Run("list");
			_consoleMock.Verify(console => console.WriteLine("List command excecuted successfully."), Times.Once);
		}

	}
}