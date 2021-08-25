using System;
using Moq;
using paranoid.software.ephemeral.MySql;
using piper.cli;
using piper.cli.Persistence;
using Xunit;

namespace tests.isolated_integrations
{

	public class ListCommandShould: IDisposable
	{
		private readonly Mock<IConsoleOutputWriter> _consoleMock;
		private readonly PiperSettings _settings;
		private readonly Cli _sut;

		public ListCommandShould()
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
		public void ShowListingSnippetsMessage()
		{
			using var db = new MySqlEphemeralDb(_settings.DbContext.GetConnectionString(false))
			                  .AddScriptFromFile("isolated-integrations/sql-files/snippets-table-creation-script.sql")
			                  .AddScriptFromFile("isolated-integrations/sql-files/snippets-table-mock-data.sql")
			                  .Build();
			_settings.DbContext.DatabaseName = db.Name;
			_sut.Run("list");
			_consoleMock.Verify(console => console.WriteLine($"Listing 3 snippets."), Times.Once);
		}
		
		[Fact]
		public void ListSnippetsFileNames()
		{
			using var db = new MySqlEphemeralDb(_settings.DbContext.GetConnectionString(false))
			               .AddScriptFromFile("isolated-integrations/sql-files/snippets-table-creation-script.sql")
			               .AddScriptFromFile("isolated-integrations/sql-files/snippets-table-mock-data.sql")
			               .Build();
			_settings.DbContext.DatabaseName = db.Name;
			_sut.Run("list");
			_consoleMock.Verify(console => console.WriteLine(It.Is<string>(s=>s.EndsWith(".html"))), Times.Exactly(3));
		}

		[Fact]
		public void ShowListCommandSuccesMessage()
		{
			using var db = new MySqlEphemeralDb(_settings.DbContext.GetConnectionString(false))
			               .AddScriptFromFile("isolated-integrations/sql-files/snippets-table-creation-script.sql")
			               .AddScriptFromFile("isolated-integrations/sql-files/snippets-table-mock-data.sql")
			               .Build();
			_settings.DbContext.DatabaseName = db.Name;
			_sut.Run("list");
			_consoleMock.Verify(console => console.WriteLine("List command excecuted successfully."), Times.Once);
		}

		public void Dispose()
		{
			
		}
	}
}