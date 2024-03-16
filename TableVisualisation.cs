using System.Data;
using Spectre.Console;

namespace CodingTracker
{
    internal class TableVisualisation
    {
        internal void ShowTable(List<CodingSession> tableData)
        {
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