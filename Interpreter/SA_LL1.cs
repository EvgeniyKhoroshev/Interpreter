using Interpreter.Constants;
using Interpreter.Extensions;
using Interpreter.Translation;
using Interpreter.TranslationResult;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Interpreter
{
    internal class SA_LL1
    {
        private readonly int[,] _LLTable;
        private readonly char[] _LLColumn;
        private readonly string[] _LLRow;
        
        private Stack WorkStack, OutputStack;
        public string Log;
        private Queue<LexicalToken> input;
        private LexicalToken buffer = new LexicalToken();
        public Queue<string> errLog;
        private Queue<LexicalToken> Tokens;
        public SA_LL1(Queue<LexicalToken> input)
        {
            _LLRow = (string[])FileExtensions.ReadFromFile(typeof(string[]), PathConstants.LL1RowPath);
            _LLColumn = (char[])FileExtensions.ReadFromFile(typeof(char[]), PathConstants.LL1ColumnPath);
            _LLTable = (int[,])FileExtensions.ReadFromFile(typeof(int[,]), PathConstants.LL1TablePath);
            errLog = new Queue<string>();
            Tokens = input;
        }

        public Stack Do()
        {
            input = Tokens;
            buffer.Token = TranslationToken.EndOfProgram;
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
                    c = (int)buffer.Token - 1;
                    if (r != -1 && -1 != c)
                    {
                        param = _LLTable[r, c];
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

        private int GetRowByID(string Row)
        {
            if (_LLRow.Contains(Row))
            {
                for (int i = 0; i < _LLRow.Length; ++i)
                    if (Row == _LLRow[i])
                        return i;
            }
            return -1;
        }

        private int GetColByID(char Col)
        {
            if (!_LLColumn.Contains(Col))
                return -1;

            for (int i = 0; i < _LLRow.Length; ++i)
                if (Col == _LLColumn[i]) return i;

            return -1;
        }

        private void PutInStack(int caseOf)
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
                    errLog.Enqueue("Строка [" + (buffer.StringNumber + 1).ToString() + "]. Неожиданный символ " + buffer.Value + ". \n");
                    PrintLog(0);
                    if (input.Count > 0)
                        buffer = input.Dequeue();
                    break;
            }
        }

        private void PrintLog(int Rule)
        {
            OutputStack.Push(WorkStack.Pop());
            Log += "M (" + OutputStack.Peek() + " , " + buffer.Token + ") = " + Convert.ToString(Rule) + "\n";

        }

    }

}
