using Spectre.Console;

namespace CodingTracker
{
    internal class TableVisualisation
    {
        internal void ShowTable(List<CodingSession> tableData)
        {
            foreach (var element in tableData)
            {
                string date = element.Date;
                string substring1 = date.Substring(0, 2);
                string substring2 = date.Substring(2, 2);
                string substring3 = date.Substring(4, 2);

                date = substring1 + '-' + substring2 + '-' + substring3;
                string[] splitDate = date.Split('-');
                Array.Reverse(splitDate);

                element.Date = string.Join('-', splitDate);
            }
            Console.WriteLine("\n\n");
            var table = new Table();

            table.Title("[green]Coding[/]");

            table.AddColumn("[blue]Id[/]");
            table.AddColumn("[blue]Date[/]");
            table.AddColumn("[blue]Duration[/]");

            foreach (var session in tableData)
            {
                table.AddRow($"[tan]{session.Id}[/]", $"[tan]{session.Date}[/]", $"[tan]{session.Duration}[/]");
            }

            AnsiConsole.Write(table);
        }

        internal void ShowFilterTable(List<CodingSession> tableData)
        {
            foreach (var element in tableData)
            {
                string date = element.Date;
                string substring1 = date.Substring(0, 2);
                string substring2 = date.Substring(2, 2);
                string substring3 = date.Substring(4, 2);

                date = substring1 + '-' + substring2 + '-' + substring3;
                string[] splitDate = date.Split('-');
                Array.Reverse(splitDate);

                element.Date = string.Join('-', splitDate);
            }
            Console.WriteLine("\n\n");
            var table = new Table();

            table.Title("[green]Coding[/]");

            table.AddColumn("[blue]Id[/]");
            table.AddColumn("[blue]Date[/]");
            table.AddColumn("[blue]Duration[/]");

            foreach (var session in tableData)
            {
                table.AddRow($"[tan]{session.Id}[/]", $"[tan]{session.Date}[/]", $"[tan]{session.Duration}[/]");
            }

            AnsiConsole.Write(table);
        }
    }
}