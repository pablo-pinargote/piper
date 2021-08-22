using System;
using piper.cli.Persistence;

namespace piper.cli
{

	public class PiperSettings: ICloneable
	{
		public MySqlDbContext DbContext { get; set; } = new MySqlDbContext();
		public int? MaxRevisionsToKeep { get; set; }
		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}