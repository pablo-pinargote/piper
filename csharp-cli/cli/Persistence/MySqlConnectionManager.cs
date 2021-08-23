namespace piper.cli.Persistence
{
	public class MySqlConnectionManager
	{
		protected readonly MySqlDbContext Context;

		protected MySqlConnectionManager(MySqlDbContext context)
		{
			Context = context;
		}
	}
}