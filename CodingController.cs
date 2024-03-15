using System.Configuration;
using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    internal class CodingController
    {
        static string? connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        internal void Get()
        {
            TableVisualisation tableVisualisation = new();
            List<CodingSession> tableData = new();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"SELECT * FROM coding_hours";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new CodingSession{
                            Id = reader.GetInt32(0),
                            Date = reader.GetString(1),
                            Duration = reader.GetString(2)
                        });
                    }
                }
                else
                    Console.WriteLine("No rows found.");

                tableVisualisation.ShowTable(tableData);
            }
        }

        internal void Post(CodingSession coding)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO coding_hours (date, duration) VALUES ('{coding.Date}', '{coding.Duration}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal void Delete()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal void Update()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}