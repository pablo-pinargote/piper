using FluentAssertions;
using Xunit;

namespace tests.isolated_units.Persistence
{
	public class WhenGetMySqlDatabaseConnectionString
	{
		[Fact]
		public void ShouldIncludeDatabaseParameter()
		{
			var sut = new piper.cli.Persistence.MySqlDbContext
			          {
				          HostName = "localhost",
				          DatabaseName = "test",
				          Password = "",
				          Username = "root",
				          PortNumber = 3306
			          };
			sut.GetConnectionString().Should().Contain("database");
		} 
	}
}