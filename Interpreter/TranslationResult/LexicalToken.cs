using Interpreter.Translation;

namespace Interpreter.TranslationResult
{
    /// <summary>
    /// Класс токена, который получился в результате разбора лексемы.
    /// </summary>
    public class LexicalToken
    {
        /// <summary>
        /// Индекс начала лексемы.
        /// </summary>
        public int LexemeStartIndex { get => CurrentColumnIndex - Value.Length; }

        /// <summary>
        /// Значение лексемы.
        /// </summary>
        public string Value;

        /// <summary>
        /// Токен.
        /// </summary>
        public TranslationToken Token;


        public string AttributeValue;

        /// <summary>
        /// Номер строки.
        /// </summary>
        public int StringNumber;

        /// <summary>
        /// Текущий номер столбца.
        /// </summary>
        public int CurrentColumnIndex;

        public bool isIdentifier;
        public bool isConditionalBranch;
    }

    public static class LexicalTokenExtensions
    {
        public static void Clear(this LexicalToken source)
        {
            source.CurrentColumnIndex = 0;
            source.Token = TranslationToken.Space;
            source.Value = "";
            source.StringNumber = 0;
            source.AttributeValue = "";
            source.isIdentifier = false;
            source.isConditionalBranch = false;
        }
    }
}
