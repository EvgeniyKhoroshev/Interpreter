using Interpreter.Translation;
using Interpreter.TranslationResult;
using System.Collections.Generic;

namespace Interpreter
{
    public class TranslationTable
    {
        public id_Table Identifiers = new id_Table();
        public TranslationTable() => Buffer = new LexicalToken();

        public Queue<LexicalToken> TranslationList = new Queue<LexicalToken>();

        /// <summary>
        /// Структура токена, который получился в результате разбора.
        /// </summary>
        public LexicalToken Buffer; // Буфер ввода/вывода таблицы
        public void InsertTranslationResult()
        {
            if (Buffer.isIdentifier)
                Identifiers.addRecord(Buffer);
            else if (Buffer.Token == TranslationToken.Identifier)
                Identifiers.isIdentifierExists(Buffer);
            TranslationList.Enqueue(Buffer);
        }
    }
}
