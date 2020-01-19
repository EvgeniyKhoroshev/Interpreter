namespace Interpreter.Constants
{
    public static class PathConstants
    {
        public static string LexicalAnalyzerRows = $"{TablesPath}\\LexicalAnalyzer\\RowKeys.txt";
        public static string LexicalTablePath = $"{TablesPath}\\LexicalAnalyzer\\Table.txt";

        public static string LL1TablePath = $"{TablesPath}\\LL1\\Table.txt";
        public static string LL1RowPath = $"{TablesPath}\\LL1\\RowKeys.txt";
        public static string LL1ColumnPath = $"{TablesPath}\\LL1\\ColumnKeys.txt";

        public static string LR1ColumnPath = $"{TablesPath}\\LR1\\ColumnKeys.txt";
        public static string LR1TablePath = $"{TablesPath}\\LR1\\Table.txt";


        private const string TablesPath = "..\\..\\Resources\\Tables\\";
    }
}
