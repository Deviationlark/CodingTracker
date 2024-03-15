using ConsoleTableExt;

namespace CodingTracker
{
    internal class TableVisualisation
    {
        internal void ShowTable<T>(List<T> tableData) where T : class
        {
            Console.WriteLine("\n\n");

            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Coding")
                .ExportAndWriteLine();
            
            Console.WriteLine("\n\n");
        }
    }
}