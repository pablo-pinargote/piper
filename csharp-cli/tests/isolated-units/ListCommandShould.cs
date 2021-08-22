using System.Collections.Generic;
using System.Linq;
using Moq;
using piper.cli;
using piper.cli.Persistence;
using Xunit;

namespace tests.isolated_units
{
	public class ListCommandShould
	{
		
		private readonly Mock<IConsoleOutputWriter> _consoleMock;
		private readonly List<Snippet> _allSnippets;
		private readonly Cli _sut;

		public ListCommandShould()
		{
			_consoleMock = new Mock<IConsoleOutputWriter>();
			_allSnippets = new List<Snippet>
			               {
				               new Snippet
				               {
					               Name = "_common",
					               Html = "<div>_common snippet</div>"
				               },
				               new Snippet
				               {
					               Name = "nav-bar-plugster",
					               Html = "<div>nav-bar-plugster snippet</div>"
				               }
			               };
			var repositoryMock = new Mock<ISnippetsRepository>(MockBehavior.Strict);
			repositoryMock.Setup(m => m.GetAllNames()).Returns(_allSnippets.Select(a => a.Name).ToList);
			_sut = new Cli(_consoleMock.Object, null, repositoryMock.Object);
		}

		[Fact]
		public void ShowListingSnippetsMessage()
		{
			_sut.Run("list");
			_consoleMock.Verify(console => console.WriteLine($"Listing {_allSnippets.Count} snippets."), Times.Once);
		}

		[Fact]
		public void ListSnippetsFileNames()
		{
			_sut.Run("list");
			_consoleMock.Verify(console => console.WriteLine(It.Is<string>(s=>s.EndsWith(".html"))), Times.Exactly(_allSnippets.Count));
		}

		[Fact]
		public void ShowListCommandSuccesMessage()
		{
			_sut.Run("list");
			_consoleMock.Verify(console => console.WriteLine("List command excecuted successfully."), Times.Once);
		}

	}
}