using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace Int_something
{
    class SA_LL1
    {
        private int[,] LLTable =
        { // S   F   d   B   X   W   D   I   T   E   C   f   i   o   c   ;   ,   =   +   -   *   /   %   (   )   $
{   1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   5,  2,  2,  5,  5,  5,  5,  5,  0,  0,  0,  0,  0,  0,  50, 0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  9,  8,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 4,  50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50  },
{   13, 0,  0,  14, 15, 24, 27, 28, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 7,  50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50  },
{   10, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 12, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50  },
{   29, 0,  0,  0,  0,  0,  0,  0,  29, 0,  0,  0,  0,  0,  0,  29, 0,  0,  0,  0,  0,  0,  0,  0   },
{   16, 17, 17, 16, 16, 16, 16, 16, 0,  0,  0,  0,  0,  18, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   50, 50, 50, 50, 50, 50, 50, 50, 50, 20, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50  },
{   21, 22, 22, 21, 21, 21, 21, 21, 0,  0,  0,  0,  0,  23, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   33, 0,  0,  0,  0,  0,  0,  0,  33, 0,  0,  0,  0,  0,  0,  33, 0,  0,  0,  0,  0,  0,  0,  0   },
{   25, 0,  0,  25, 25, 25, 25, 25, 0,  0,  0,  0,  0,  26, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 30, 31, 50, 50, 50, 50  },
{   38, 0,  0,  0,  0,  0,  0,  0,  39, 0,  0,  0,  0,  0,  0,  42, 0,  0,  0,  41, 0,  0,  0,  0   },
{   50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 34, 35, 36, 50  },
{   0, 0,  0,  0,  0,  0,  0,  0,  43, 0,  0,  0,  0,  0,  0,  43, 0,  0,  0,  0,  0,  0,  0,  0   },
{   100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0,  0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  100,    0   },
{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  999    },

};
        Stack WorkStack, OutputStack;
        char[] LLColumn = {'X', 'B', 'd', 'R', 'I', 'W', 'i', 'e', 'C', 'E', 'c', ';', ',', '{',
            '}', '(', ')', '=', '+', '-', '*', '/', '%','$' };
        string[]LLRow={"<P>", "<POp>", "<O>", "<a>", "<Op>", "<b>", "<LID>", "<c>", "<V>", "<d>",
            "<e>", "<f>", "<F>", "<g>", "<h>", "<PV>", "<o>", "<Cmp>", "X", "B", "d", "R", "I", "W",
            "i", "e", "C", "E", "c", ";", ",", "{", "}", "(", ")", "=", "+", "-", "*", "/",	"%","$" };
        public string Log;
        Queue<LL1LexilacAnalyzer.TranslatedToken> input;
        LL1LexilacAnalyzer.TranslatedToken buffer;
        public Queue<string> errLog;
        Queue<LL1LexilacAnalyzer.TranslatedToken> Tokens;
        public SA_LL1(Queue<LL1LexilacAnalyzer.TranslatedToken> input)
        {
            errLog = new Queue<string>();
            Tokens = input;
        }

        internal PostfixNotation toPostfix
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public Stack Do()
        {
            input = Tokens;
            buffer.Token = '$';
            input.Enqueue(buffer);
            
            int c, r;
            WorkStack = new Stack();
            OutputStack = new Stack();
            int param;
            WorkStack.Push("$");
            WorkStack.Push("<P>");
            buffer = input.Dequeue();
            while (WorkStack.Count > 0)
            {
                try
                {
                    r = GetRowByID(Convert.ToString(WorkStack.Peek()));
                    c = GetColByID(buffer.Token);
                    if (r != -1 && -1 != c)
                    {
                        param = LLTable[r, c];
                        PutInStack(param);
                    }
                    else
                    {
                        buffer = input.Dequeue();
                        PrintLog(0);
                    }
                }
                catch (IndexOutOfRangeException)
                { }
            }
            return OutputStack;
        }
        int GetRowByID(string Row)
        {
            if (LLRow.Contains(Row))
            {
                for (int i = 0; i < LLRow.Length; ++i)
                    if (Row == LLRow[i])
                        return i;
            }
            return -1;
        }
        int GetColByID(char Col)
        {
            if (!LLColumn.Contains(Col))
                return -1;

            for (int i = 0; i < LLRow.Length; ++i)
                if (Col == LLColumn[i]) return i;

            return -1;
        }
        void PutInStack(int caseOf)
        {
            switch (caseOf)
            {
                case 1:
                    PrintLog(caseOf);
                    WorkStack.Push("}");
                    WorkStack.Push("<POp>");
                    WorkStack.Push("{");
                    WorkStack.Push("X");
                    break;
                case 2:
                    PrintLog(caseOf);
                    WorkStack.Push("<a>");
                    WorkStack.Push("<O>");
                    break;
                case 4:
                    PrintLog(caseOf);
                    WorkStack.Push("<POp>");
                    WorkStack.Push(";");
                    break;
                case 5:
                    PrintLog(caseOf);
                    WorkStack.Push("<b>");
                    WorkStack.Push("<Op>");
                    break;
                case 7:
                    PrintLog(caseOf);
                    WorkStack.Push("<POp>");
                    WorkStack.Push(";");
                    break;
                case 8:
                    PrintLog(caseOf);
                    WorkStack.Push("<LID>");
                    WorkStack.Push("d");
                    break;
                case 9:
                    PrintLog(caseOf);
                    WorkStack.Push("<LID>");
                    WorkStack.Push("B");
                    break;
                case 10:
                    PrintLog(caseOf);
                    WorkStack.Push("<c>");
                    WorkStack.Push("X");
                    break;
                case 12:
                    PrintLog(caseOf);
                    WorkStack.Push("<LID>");
                    WorkStack.Push(",");
                    break;
                case 13:
                    PrintLog(caseOf);
                    WorkStack.Push("<V>");
                    WorkStack.Push("=");
                    WorkStack.Push("X");
                    break;
                case 14:
                    PrintLog(caseOf);
                    WorkStack.Push("R");
                    break;
                case 15:
                    PrintLog(caseOf);
                    WorkStack.Push("<d>");
                    WorkStack.Push("<Cmp>");
                    WorkStack.Push("I");
                    break;
                case 16:
                    PrintLog(caseOf);
                    WorkStack.Push("<e>");
                    WorkStack.Push("<Op>");
                    break;
                case 17:
                    PrintLog(caseOf);
                    WorkStack.Push("<e>");
                    WorkStack.Push("<O>");
                    break;
                case 18:
                    PrintLog(caseOf);
                    WorkStack.Push("<e>");
                    WorkStack.Push("}");
                    WorkStack.Push("<POp>");
                    WorkStack.Push("{");
                    break;
                case 20:
                    PrintLog(caseOf);
                    WorkStack.Push("<f>");
                    WorkStack.Push("E");
                    WorkStack.Push(";");
                    break;
                case 21:
                    PrintLog(caseOf);
                    WorkStack.Push("<Op>");
                    break;
                case 22:
                    PrintLog(caseOf);
                    WorkStack.Push("<O>");
                    break;
                case 23:
                    PrintLog(caseOf);
                    WorkStack.Push("}");
                    WorkStack.Push("<POp>");
                    WorkStack.Push("{");
                    break;
                case 24:
                    PrintLog(caseOf);
                    WorkStack.Push("<g>");
                    WorkStack.Push("<Cmp>");
                    WorkStack.Push("W");
                    break;
                case 25:
                    PrintLog(caseOf);
                    WorkStack.Push("<Op>");
                    break;
                case 26:
                    PrintLog(caseOf);
                    WorkStack.Push("}");
                    WorkStack.Push("<POp>");
                    WorkStack.Push("{");
                    break;
                case 27:
                    PrintLog(caseOf);
                    WorkStack.Push("X");
                    WorkStack.Push("i");
                    break;
                case 28:
                    PrintLog(caseOf);
                    WorkStack.Push("<V>");
                    WorkStack.Push("e");
                    break;
                case 29:
                    PrintLog(caseOf);
                    WorkStack.Push("<h>");
                    WorkStack.Push("<F>");
                    break;
                case 30:
                    PrintLog(caseOf);
                    WorkStack.Push("<F>");
                    WorkStack.Push("+");
                    break;
                case 31:
                    PrintLog(caseOf);
                    WorkStack.Push("<F>");
                    WorkStack.Push("-");
                    break;
                case 33:
                    PrintLog(caseOf);
                    WorkStack.Push("<o>");
                    WorkStack.Push("<PV>");
                    break;
                case 34:
                    PrintLog(caseOf);
                    WorkStack.Push("<PV>");
                    WorkStack.Push("*");
                    break;
                case 35:
                    PrintLog(caseOf);
                    WorkStack.Push("<PV>");
                    WorkStack.Push("/");
                    break;
                case 36:
                    PrintLog(caseOf);
                    WorkStack.Push("<PV>");
                    WorkStack.Push("%");
                    break;
                case 38:
                    PrintLog(caseOf);
                    WorkStack.Push("X");
                    break;
                case 39:
                    PrintLog(caseOf);
                    WorkStack.Push("C");
                    break;
                case 40:
                    PrintLog(caseOf);
                    WorkStack.Push("X");
                    WorkStack.Push("-");
                    break;
                case 41:
                    PrintLog(caseOf);
                    WorkStack.Push("C");
                    WorkStack.Push("-");
                    break;
                case 42:
                    PrintLog(caseOf);
                    WorkStack.Push(")");
                    WorkStack.Push("<V>");
                    WorkStack.Push("(");
                    break;
                case 43:
                    PrintLog(caseOf);
                    WorkStack.Push(")");
                    WorkStack.Push("<V>");
                    WorkStack.Push("c");
                    WorkStack.Push("<V>");
                    WorkStack.Push("(");
                    break;
                case 50:
                    PrintLog(caseOf);
                    break;
                case 100:
                    PrintLog(caseOf);
                    buffer = input.Dequeue();
                    break;
                case 999:
                    PrintLog(caseOf);
                    Log += "Синтаксический разбор успешно завершен.\n";
                    return;
                case 0:
                    errLog.Enqueue("Строка ["+(buffer.StringNumber+1).ToString()+"]. Неожиданный символ "+buffer.Value+". \n");
                    PrintLog(0);
                    if (input.Count>0)
                        buffer = input.Dequeue();
                    break;
            }
        }

        void PrintLog(int Rule)
        {
            OutputStack.Push(WorkStack.Pop());
            Log += "M (" + OutputStack.Peek() + " , " + buffer.Token + ") = " + Convert.ToString(Rule) + "\n";

        }

    }

}
