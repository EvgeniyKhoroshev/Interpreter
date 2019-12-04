namespace Interpreter.Constants
{
    public static class PathConstants
    {
        public static string LexicalTablePath = $"{TablesPath}LexicalAnalyzerTable.txt";

        public static string LL1TablePath = $"{TablesPath}\\LLTables\\LL1Table.txt";
        public static string LL1RowPath = $"{TablesPath}\\LLTables\\LL1Row.txt";
        public static string LL1ColumnPath = $"{TablesPath}\\LLTables\\LL1Column.txt";

        public static string LR1ColumnPath = $"{TablesPath}\\LRTables\\LR1Column.txt";
        public static string LR1TablePath = $"{TablesPath}\\LRTables\\LR1Table.txt";


        private const string TablesPath = "..\\..\\..\\Tables\\";
    }
}
