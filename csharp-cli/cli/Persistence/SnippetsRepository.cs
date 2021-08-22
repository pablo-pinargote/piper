using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace piper.cli.Persistence
{
	public interface ISnippetsRepository
	{
		IEnumerable<string> GetAllNames();
		IEnumerable<Snippet> GetAll();
		IEnumerable<Snippet> GetByNames(IEnumerable<string> names);
		Snippet GetMe(string name);
		void UpdateHtmlContent(string name, string html);

	}
	public class SnippetsRepository: MySqlConnectionManager, ISnippetsRepository
	{
		public SnippetsRepository(MySqlDbContext context) : base(context)
		{
		}

		public IEnumerable<string> GetAllNames()
		{
			using var cnn = new MySqlConnection(_context.GetConnectionString());
			cnn.Open();
			using var cmd = new MySqlCommand(@"select name from djangocms_snippet_snippet", cnn);
			using var reader = cmd.ExecuteReader();
			while (reader.Read())
				yield return reader["name"].ToString();
		}

		public IEnumerable<Snippet> GetAll()
		{
			using var cnn = new MySqlConnection(_context.GetConnectionString());
			cnn.Open();
			using var cmd = new MySqlCommand(@"select name, html from djangocms_snippet_snippet", cnn);
			using var reader = cmd.ExecuteReader();
			while (reader.Read())
				yield return new Snippet {Name = reader["name"].ToString(), Html = reader["html"].ToString()};
		}

		public IEnumerable<Snippet> GetByNames(IEnumerable<string> names)
		{
			using var cnn = new MySqlConnection(_context.GetConnectionString());
			cnn.Open();
			using var cmd = new MySqlCommand(@"select name, html from djangocms_snippet_snippet  where find_in_set(name, @names) != 0", cnn);
			cmd.Parameters.AddWithValue("@names", string.Join(",", names));
			using var reader = cmd.ExecuteReader();
			while (reader.Read())
				yield return new Snippet {Name = reader["name"].ToString(), Html = reader["html"].ToString()};
		}

		public Snippet GetMe(string name)
		{
			using var cnn = new MySqlConnection(_context.GetConnectionString());
			cnn.Open();
			using var cmd = new MySqlCommand(@"select name, html from djangocms_snippet_snippet  where name = @name", cnn);
			cmd.Parameters.AddWithValue("@name", string.Join(",", name));
			using var reader = cmd.ExecuteReader();
			return reader.Read() ? new Snippet {Name = reader["name"].ToString(), Html = reader["html"].ToString()} : null;
		}

		public void UpdateHtmlContent(string name, string html)
		{
			using var cnn = new MySqlConnection(_context.GetConnectionString());
			using var cmd =
				new MySqlCommand(
					@"update djangocms_snippet_snippet set html=@html, template=@template, slug=@slug where name = @name",
					cnn);
			cmd.Parameters.AddWithValue("@name", name);
			cmd.Parameters.AddWithValue("@html", html);
			cmd.ExecuteNonQuery();
		}
		
	}
}