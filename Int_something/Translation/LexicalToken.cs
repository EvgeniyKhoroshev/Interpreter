using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int_something.Translation
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
