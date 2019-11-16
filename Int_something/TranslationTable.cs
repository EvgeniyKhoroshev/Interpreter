using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Int_something
{
    class TranslationTable
    {
        public id_Table Identifiers = new id_Table();
        public TranslationTable()
        {
            Buffer.numberInProgram = 0;
            Buffer.Token = ' ';
            Buffer.Value = "";
            Buffer.Lexeme = 0;
            Buffer.StringNumber = 0;
            Buffer.AttributeValue = "";
            Buffer.isIdentifier = false;
        }

        public Queue<translationTable> TranslationList = new Queue<translationTable>();
        public struct translationTable // Таблица трансляции
        {
            public int numberInProgram;
            public string Value;
            public int Lexeme;
            public char Token;
            public string AttributeValue;
            public int StringNumber;
            public bool isIdentifier;
            public bool isConditionalBranch;

        };
        public translationTable Buffer; // Буфер ввода/вывода таблицы
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
