using System.Configuration;
using ConsoleTableExt;
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

                tableCmd = $"SELECT * FROM coding_hours ORDER BY date";

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

        internal void Filter(int startDate, int endDate)
        {
            GetUserInput getUserInput = new();
            TableVisualisation tableVisualisation = new();
            List<CodingSession> tableData = new();
            List<CodingSession> filterData = new();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string tableCmd = $"SELECT * FROM coding_hours";

                tableData = connection.Query<CodingSession>(tableCmd).ToList();

                connection.Close();
            }
            int[] dateRecords = new int[tableData.Count];
            int count = 0;
            foreach (var record in tableData)
            {
                string? date = record.Date.ToString();
                string[] dates = date.Split('-');
                Array.Reverse(dates);
                string time = "";
                foreach (var element in dates)
                {
                    time += element;
                }
                dateRecords[count] = Convert.ToInt32(time);
                count++;
            }
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "SELECT EXISTS(SELECT date FROM coding_hours)";

                int rowCount = Convert.ToInt32(tableCmd.ExecuteScalar());

                if (rowCount == 0)
                {
                    Console.WriteLine("No dates found. Press Enter to go back to main menu");
                    Console.ReadLine();
                    getUserInput.MainMenu();
                }
                count = 0;
                foreach (var element in tableData)
                {
                    tableCmd.CommandText = $"UPDATE coding_hours SET date = '{dateRecords[count]}' WHERE Id = '{element.Id}'";
                    tableCmd.ExecuteNonQuery();
                    count++;
                }
                connection.Close();
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string tableCmd = $"SELECT * FROM coding_hours WHERE date BETWEEN '{startDate}' AND '{endDate}' ORDER BY date";

                filterData = connection.Query<CodingSession>(tableCmd).ToList();

                connection.Close();
            }
            if (tableData.Count == 0)
            {
                Console.WriteLine("No records found. Press enter to go back to main menu.");
                Console.ReadLine();
            }
            if (tableData.Count > 0)
                tableVisualisation.ShowFilterTable(filterData);
        }

        internal string[] Report()
        {
            // convert duration records to integers and add them up then show them
            List<CodingSession> tableData = new();
            string[] info = new string[2];
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string tableCmd = $"SELECT * FROM coding_hours";

                tableData = connection.Query<CodingSession>(tableCmd).ToList();

                connection.Close();
            }
            TimeSpan duration;
            TimeSpan totalDuration = new TimeSpan(0, 0, 0);
            foreach (var element in tableData)
            {
                duration = TimeSpan.Parse(element.Duration);
                totalDuration += duration;
            }

            TimeSpan averageDuration;

            averageDuration = TimeSpan.FromSeconds(totalDuration.TotalSeconds / tableData.Count);

            info[0] = totalDuration.ToString();
            info[1] = averageDuration.ToString();

            return info;

        }

        internal void InsertGoal(Goals goals)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO coding_hours (hours) VALUES ('{goals.Hours}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}