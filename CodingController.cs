using System.Configuration;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    internal class CodingController
    {
        static string? connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        internal void Get()
        {
            GetUserInput userInput = new();
            TableVisualisation tableVisualisation = new();
            List<CodingSession> tableData = new();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string tableCmd;

                tableCmd = $"SELECT * FROM coding_hours";

                tableData = connection.Query<CodingSession>(tableCmd).ToList();

                connection.Close();
            }
            Console.Clear();

            if (tableData.Count == 0)
            {
                Console.WriteLine("No records found. Press enter to go back to main menu.");
                Console.ReadLine();
            }
            if (tableData.Count > 0)
            tableVisualisation.ShowTable(tableData);
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

        internal int Delete(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE FROM coding_hours WHERE Id = '{id}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                connection.Close();

                return rowCount;
            }
        }

        internal void Update(int id)
        {
            GetUserInput userInput = new();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_hours WHERE Id = '{id}')";

                int rowCount = Convert.ToInt32(tableCmd.ExecuteScalar());

                if (rowCount == 0)
                {
                    Console.WriteLine($"Record with id {id} doesn't exist.");
                    Console.ReadLine();
                    userInput.ProcessUpdate();
                }

                string date = userInput.GetDateInput();
                string[] info = userInput.CalculateDuration();

                tableCmd.CommandText = $"UPDATE coding_hours SET date = '{date}', duration = '{info[2]}' WHERE Id = {id}";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}