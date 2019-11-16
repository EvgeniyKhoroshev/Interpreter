using Int_something.TranslationResult;
using System.Collections.Generic;

namespace Int_something
{
    public class LL1LexilacAnalyzer
    {
        public id_Table Identifiers = new id_Table();
        public LL1LexilacAnalyzer() => Buffer = new LexicalToken()
        { Buffer.Token = ' '  };

        public Queue<LexicalToken> TranslationList = new Queue<LexicalToken>();

        /// <summary>
        /// Структура токена, который получился в результате разбора.
        /// </summary>

        public LexicalToken Buffer; // Буфер ввода/вывода таблицы
        public void Put()
        {
            if (Buffer.isIdentifier)
                Identifiers.addRecord(Buffer);
            else if(Buffer.Token == 'X')
                Identifiers.isIdentifierExists(Buffer);
            TranslationList.Enqueue(Buffer);
            ClearBuffer();
        }
        public void ClearBuffer() //Очистка буфера
        {
            Buffer.numberInProgram = 0;
            Buffer.Token = ' ';
            Buffer.Value = "";
            Buffer.Lexeme = 0;
            Buffer.StringNumber = 0;
            Buffer.AttributeValue = "";
            Buffer.isIdentifier = false;
            Buffer.isConditionalBranch = false;

        }

        internal id_Table id_Table
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal SA_LL1 SA_LL1
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }
}
