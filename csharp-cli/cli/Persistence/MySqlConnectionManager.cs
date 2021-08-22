namespace piper.cli.Persistence
{
	public class MySqlConnectionManager
	{
		protected readonly MySqlDbContext _context;

		protected MySqlConnectionManager(MySqlDbContext context)
		{
			_context = context;
		}
	}
}