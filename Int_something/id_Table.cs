using Int_something.TranslationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Int_something
{
    public class id_Table
    {

        public Dictionary<string, ID_int> intTable = new Dictionary<string, ID_int>();
        public Dictionary<string, ID_bool> boolTable = new Dictionary<string, ID_bool>();
        public Queue<string> errors = new Queue<string>();
        public struct ID_bool
        {
            public int numberInProgram;
            public string name;
            public bool value;
        }
        public struct ID_int
        {
            public int numberInProgram;
            public string name;
            public int value;
        }
        ID_int int_buff;
        ID_bool bool_buff;
        void clearBuff()
        {
            int_buff.name = "";
            int_buff.numberInProgram = 0;
            int_buff.value = 0;
            bool_buff.name = "";
            bool_buff.numberInProgram = 0;
            bool_buff.value = false;
        }
        public bool addRecord(LexicalToken input) // Возвращает "true", если идентификатор/константа успешно занесен(-на) в таблицу.
        {

            string nameForBuf;
            if (input.isIdentifier)
            {
                nameForBuf = input.Value;
                if (!intTable.ContainsKey(nameForBuf) && !boolTable.ContainsKey(nameForBuf))
                {
                    if (input.AttributeValue == "BOOL")
                    {
                        bool_buff.numberInProgram = input.numberInProgram;
                        bool_buff.name = nameForBuf;
                        bool_buff.value = false;
                        boolTable.Add(nameForBuf, bool_buff);
                        clearBuff();
                        return true;
                    }
                    if (input.AttributeValue == "INT")
                    {
                        int_buff.numberInProgram = input.numberInProgram;
                        int_buff.name = nameForBuf;
                        int_buff.value = 0;
                        intTable.Add(nameForBuf, int_buff);
                        clearBuff();
                        return true;
                    }
                }
                else
                {
                    errors.Enqueue("Строка [" + (input.StringNumber + 1).ToString() + "][" + input.numberInProgram + "] '" + input.Value + "' Идентификатор с таким именем уже существует.\n");
                    return false;
                }
            }
            return false;
        }
        public bool isIdentifierExists(LexicalToken input)
        {
            string nameForBuf = input.Value;
            if (!intTable.ContainsKey(nameForBuf) && !boolTable.ContainsKey(nameForBuf))
            {
                errors.Enqueue("[" + (input.StringNumber + 1).ToString() + "][" + input.numberInProgram + "] '" + input.Value + "' Переменная не была обьявлена в программе.\n");
                return false;
            }
            return true;
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
