using Dapper;
using Npgsql;

namespace PlantApp
{
	public class DatabaseHelper
	{
		private readonly string _connString = "Host=localhost;Username=postgres;Password=postgres;Database=plants";

		public DatabaseHelper()
		{
		}

		public IEnumerable<T> ExecuteQuery<T>(string sql, object? parameters = null)
		{
			using (var connection = new NpgsqlConnection(_connString))
			{
				connection.Open();
				return connection.Query<T>(sql, parameters);
			}
		}
		public int ExecuteNonQuery(string sql, object? parameters = null)
		{
			using (var connection = new NpgsqlConnection(_connString))
			{
				connection.Open();
				return connection.Execute(sql, parameters);
			}
		}
	}
}
