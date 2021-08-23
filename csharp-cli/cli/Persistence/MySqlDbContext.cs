namespace piper.cli.Persistence
{
	public class MySqlDbContext
	{
		public string HostName { get; set; }
		public int? PortNumber { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string DatabaseName { get; set; }
		
		public string GetConnectionString(bool includeDatabase=true)
		{
			var serverConnectionString = $"server={HostName};port={PortNumber};uid={Username};pwd={Password}";
			return includeDatabase ? $"{serverConnectionString};database={DatabaseName}" : serverConnectionString;
		}

	}
}