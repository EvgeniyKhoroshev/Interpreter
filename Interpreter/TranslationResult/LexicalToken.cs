using Interpreter.Translation;

namespace Interpreter.TranslationResult
{
    /// <summary>
    /// Result of the lexical translation of program code.
    /// </summary>
    public class LexicalToken
    {
        /// <summary>
        /// Lexeme start index.
        /// </summary>
        public int LexemeStartIndex { get => ColumnIndex - TokenAttributeValue.Length; }

        /// <summary>
        /// Token attribute value.
        /// </summary>
        public string TokenAttributeValue;

        /// <summary>
        /// Token.
        /// </summary>
        public TranslationToken Token;

        /// <summary>
        /// String number.
        /// </summary>
        public int StringNumber;

        /// <summary>
        /// Current olumn index.
        /// </summary>
        public int ColumnIndex;
    }

    /// <summary>
    /// Class to extend <see cref="LexicalToken"/> functionality.
    /// </summary>
    public static class LexicalTokenExtensions
    {
        /// <summary>
        /// Clearing out lexical token values.
        /// </summary>
        /// <param name="source">Lexical token.</param>
        public static void Clear(this LexicalToken source)
        {
            source.ColumnIndex = default;
            source.Token = default;
            source.TokenAttributeValue = default;
            source.StringNumber = default;
            source.TokenAttributeValue = default;
        }
    }
}
