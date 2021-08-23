using System;
using System.Linq;
using Moq;
using paranoid.software.ephemeral.MySql;
using piper.cli;
using piper.cli.Persistence;
using Xunit;

namespace tests.isolated_integrations
{
	public class PullCommandShould: IDisposable
	{
		private readonly Mock<IConsoleOutputWriter> _consoleMock;
		private readonly PiperSettings _settings;
		private readonly Cli _sut;

		public PullCommandShould()
		{
			_consoleMock = new Mock<IConsoleOutputWriter>();
			_settings = new PiperSettings
			            {
				            DbContext = new MySqlDbContext
				                        {
					                        HostName = "localhost",
					                        PortNumber = 13306,
					                        Username = "root",
					                        Password = "password"
				                        },
				            MaxRevisionsToKeep = 5
			            };
			var repo = new SnippetsRepository(_settings.DbContext);
			_sut = new Cli(_consoleMock.Object, _settings, repo);
		}

		[Fact]
		public void ShowHelpMessage()
		{
			var helpText = _sut.Commands.First(c => c.Name=="pull").GetHelpText();
			_sut.Run("pull");
			_consoleMock.Verify(console=> console.WriteLine(helpText), Times.Once);
		}

		[Theory]
		[InlineData(3, 3, "pull", "-a")]
		[InlineData(1, 1, "pull", "-s", "_common")]
		[InlineData(1, 1, "pull", "-s", "_common", "-s", "absent-snippet")]
		public void ShowSuccessResults(int expectedToSuccess, int expectedToRetrieve, params string[] args)
		{
			using var db = new MySqlEphemeralDb(_settings.DbContext.GetConnectionString(false))
			               .AddScriptFromFile("isolated-integrations/sql-files/snippets-table-creation-script.sql")
			               .AddScriptFromFile("isolated-integrations/sql-files/snippets-table-mock-data.sql")
			               .Build();
			_settings.DbContext.DatabaseName = db.DatabaseName;
			_sut.Run(args);
			_consoleMock.Verify(console=> console.WriteLine(It.Is<string>(s => s.EndsWith(".html retrieved successfully !"))), Times.Exactly(expectedToSuccess));
			_consoleMock.Verify(console=> console.WriteLine($"{expectedToRetrieve} snippets retrieved."), Times.Once());
		}

		[Theory]
		[InlineData(1, 0, "pull", "-s", "absent-snippet")]
		[InlineData(1, 1, "pull", "-s", "absent-snippet", "-s", "_common")]
		[InlineData(2, 0, "pull", "-s", "absent-snippet-1", "-s", "absent-snippet-2")]
		public void ShowFailureResults(int expectedToFail, int expectedToRetrieve, params string[] args)
		{
			using var db = new MySqlEphemeralDb(_settings.DbContext.GetConnectionString(false))
			               .AddScriptFromFile("isolated-integrations/sql-files/snippets-table-creation-script.sql")
			               .AddScriptFromFile("isolated-integrations/sql-files/snippets-table-mock-data.sql")
			               .Build();
			_settings.DbContext.DatabaseName = db.DatabaseName;
			_sut.Run(args);
			_consoleMock.Verify(console=> console.WriteLine(It.Is<string>(s => s.EndsWith(".html could not be retrieved !"))), Times.Exactly(expectedToFail));
			_consoleMock.Verify(console=> console.WriteLine($"{expectedToRetrieve} snippets retrieved."), Times.Once());
		}

		public void Dispose()
		{
		}
		
	}
}