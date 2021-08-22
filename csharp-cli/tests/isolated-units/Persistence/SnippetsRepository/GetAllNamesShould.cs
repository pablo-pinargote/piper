using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using piper.cli.Persistence;
using Xunit;

namespace tests.isolated_units.Persistence.SnippetsRepository
{
	public class GetAllNamesShould
	{
		private readonly List<Snippet> _allSnippets;
		public GetAllNamesShould()
		{
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
		}

		[Fact]
		public void ReturnAListOfStrings()
		{
			var repositoryMock = new Mock<ISnippetsRepository>(MockBehavior.Strict);
			repositoryMock.Setup(m => m.GetAllNames()).Returns(_allSnippets.Select(a => a.Name).ToList);
			var sut = repositoryMock.Object;
			sut.GetAllNames().Should().BeOfType(typeof(List<string>));
		}

		[Fact]
		public void ReturnAllSnippetsNames()
		{
			var repositoryMock = new Mock<ISnippetsRepository>(MockBehavior.Strict);
			repositoryMock.Setup(m => m.GetAllNames()).Returns(_allSnippets.Select(a => a.Name).ToList());
			var sut = repositoryMock.Object;
			sut.GetAllNames().Should().HaveCount(_allSnippets.Count);
		}

	}
}