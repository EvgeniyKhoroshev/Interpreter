using Interpreter.Translation;

namespace Interpreter.TranslationResult
{
    /// <summary>
    /// Класс токена, который получился в результате разбора лексемы.
    /// </summary>
    public class LexicalToken
    {
        /// <summary>
        /// Номер строки.
        /// </summary>
        public int LineNumber;

        /// <summary>
        /// Значение лексемы.
        /// </summary>
        public string Value;

        /// <summary>
        /// Номер лексемы.
        /// </summary>
        public int LexemeNumber { get; set; }

        /// <summary>
        /// Токен.
        /// </summary>
        public TranslationToken Token;


        public string AttributeValue;
        public int StringNumber;
        public bool isIdentifier;
        public bool isConditionalBranch;
    }

    public static class LexicalTokenExtensions
    {
        public static void Clear(this LexicalToken source)
        {
            source.LineNumber = 0;
            source.Token = TranslationToken.Space;
            source.Value = "";
            source.LexemeNumber = 0;
            source.StringNumber = 0;
            source.AttributeValue = "";
            source.isIdentifier = false;
            source.isConditionalBranch = false;
        }
    }
}
