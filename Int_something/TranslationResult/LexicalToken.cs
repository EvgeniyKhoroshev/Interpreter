namespace Int_something.TranslationResult
{
    /// <summary>
    /// Класс токена, который получился в результате разбора лексемы.
    /// </summary>
    public class LexicalToken
    {
        public int numberInProgram;
        public string Value;
        public int Lexeme;
        public char Token;
        public string AttributeValue;
        public int StringNumber;
        public bool isIdentifier;
        public bool isConditionalBranch;
    }
}
