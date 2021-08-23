using FluentAssertions;
using Xunit;

namespace tests.isolated_units.Persistence
{
	public class WhenGetMySqlServerConnectionString
	{
		[Fact]
		public void ShouldNotIncludeDatabaseParameter()
		{
			var sut = new piper.cli.Persistence.MySqlDbContext
			          {
				          HostName = "localhost",
				          DatabaseName = "test",
				          Password = "",
				          Username = "root",
				          PortNumber = 3306
			          };
			sut.GetConnectionString(false).Should().NotContain("database");
		} 
	}
}