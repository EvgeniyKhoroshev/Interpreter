namespace Interpreter.TranslationResult
{
    /// <summary>
    /// Класс токена, который получился в результате разбора лексемы.
    /// </summary>
    public class LexicalToken
    {
        public int numberInProgram;

        /// <summary>
        /// Значение лексемы.
        /// </summary>
        public string Value;

        /// <summary>
        /// 
        /// </summary>
        public int Lexeme;
        public char Token;
        public string AttributeValue;
        public int StringNumber;
        public bool isIdentifier;
        public bool isConditionalBranch;
    }

    public static class LexicalTokenExtensions
    {
        public static void Clear(this LexicalToken source)
        {
            source.numberInProgram = 0;
            source.Token = ' ';
            source.Value = "";
            source.Lexeme = 0;
            source.StringNumber = 0;
            source.AttributeValue = "";
            source.isIdentifier = false;
            source.isConditionalBranch = false;
        }
    }
}
