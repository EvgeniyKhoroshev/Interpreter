using Interpreter.Extensions;
using Interpreter.Translation;
using System;
using System.Collections;
using System.Linq;
using Interpreter.Constants;

namespace Interpreter
{
    /// <summary>
    /// Class which main responsibility is translation of code to lexemes list. 
    /// </summary>
    public class LexicalAnalyzer
    {
        /// <summary>
        /// Table consists of each lexeme state provided by _rowKeys.
        /// </summary>
        private readonly int[,] _translationTable;

        /// <summary>
        /// Main symbols of the provided language. 
        /// Each symbol has a number of states described in _translationTable. 
        /// </summary>
        private readonly char[] _rowKeys;

        /// <summary>
        /// Code of the program to analyze.
        /// </summary>
        private readonly string[] _code;
        public LexicalAnalyzer(string[] input)
        {
            _code = input;
            _translationTable = (int[,])FileExtensions.ReadFromFile(typeof(int[,]), PathConstants.LexicalTablePath);
            _rowKeys = (char[])FileExtensions.ReadFromFile(typeof(char[]), PathConstants.LexicalAnalyzerRows);
        }

        private int _state; //Переменная состояния
        private int ErrorCounter;
        private bool flag = true;
        public string stateLogLA;
        public Queue ErrorListLA = new Queue();
        private int GetRowByID(char id) // Получение строки по входному символу
        {
            if (id == '\t')
                return GetRowByID(' ');
            char buf = ' ';
            if (char.IsDigit(id))
                return 19;
            if (char.IsLetter(id))
            {
                buf = char.ToUpper(id);
                if (!_rowKeys.Contains(buf))
                    return 18;
            }


            buf = char.ToUpper(id);
            for (int i = 0; i < _rowKeys.Count(); ++i)
            {
                if (_rowKeys[i] == buf)
                    return i;
            }
            return -1;
        }
        public TranslationTable TranslateCode() //Лексический анализ
        {
            stateLogLA = "Лог состояний: \n";
            TranslationTable OutputTable = new TranslationTable(); //Выходная таблица трансляции;
            id_Table identifiers = new id_Table();
            _state = 0;
            int j = 0;
            string buf = "", lexBuf = "", atrBuf = ""; // Буфер текущей строки/лексемы
            int RowCounter = 0, Switcher;
            stateLogLA = "";
            for (int i = 0; i < _code.Count(); ++i) // Перебор входных строк
            {
                buf = _code[i] + ' ';
                j = 0;
                while (buf.Length > j) // Перебор символов текущей строки
                {
                    if ((RowCounter = GetRowByID(buf[j])) == -1)
                    {
                        ErrorListLA.Enqueue("Неизвестный символ. Строка №" + Convert.ToString(i + 1) + " символ № [" + Convert.ToString(j + 1) + "] :" + buf[j]);
                        _state = 0;
                        ++j;
                        continue;
                    }
                    Switcher = _translationTable[RowCounter, _state];
                    stateLogLA += Convert.ToString(Switcher) + ' ';
                    if (Switcher == 0)
                    {
                        ErrorListLA.Enqueue("Не удается обнаружить лексему в строке №" + Convert.ToString(i + 1) + " символ № [" + Convert.ToString(j + 1) + "] :" + buf[j]);
                        ++ErrorCounter;
                        _state = 0;
                        ++j;
                        continue;
                    }
                    if (Switcher < 100)
                    {
                        if (char.IsLetter(buf[j]))
                            lexBuf += char.ToUpper(buf[j]);
                        else
                            lexBuf += buf[j];
                        _state = _translationTable[RowCounter, _state];
                        ++j;
                        continue;
                    }
                    switch (Switcher)
                    {
                        case 101:
                            OutputTable.Buffer.Value = "BOOL";
                            OutputTable.Buffer.LexemeNumber = 1;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.BooleanDataType;
                            OutputTable.Buffer.AttributeValue = "KEYWORD";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            if (!flag)
                            {
                                atrBuf = "BOOL";
                                flag = true;
                            }
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 102:
                            OutputTable.Buffer.Value = "BREAK";
                            OutputTable.Buffer.LexemeNumber = 2;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.BreakKeyword;
                            OutputTable.Buffer.AttributeValue = "KEYWORD";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 103:
                            OutputTable.Buffer.Value = "TRUE";
                            OutputTable.Buffer.LexemeNumber = 3;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.Constant;
                            OutputTable.Buffer.AttributeValue = "CONSTANT";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 104:
                            OutputTable.Buffer.Value = "FALSE";
                            OutputTable.Buffer.LexemeNumber = 4;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.Constant;
                            OutputTable.Buffer.AttributeValue = "CONSTANT";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 105:
                            OutputTable.Buffer.Value = "WHILE";
                            OutputTable.Buffer.LexemeNumber = 5;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.WhileKeyword;
                            OutputTable.Buffer.AttributeValue = "KEYWORD";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 106:
                            OutputTable.Buffer.Value = "INPUT";
                            OutputTable.Buffer.LexemeNumber = 6;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.InputKeyword;
                            OutputTable.Buffer.AttributeValue = "KEYWORD";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 107:
                            OutputTable.Buffer.Value = "IF";
                            OutputTable.Buffer.LexemeNumber = 7;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.InputKeyword;
                            OutputTable.Buffer.AttributeValue = "KEYWORD";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 108:
                            OutputTable.Buffer.Value = "ECHO";
                            OutputTable.Buffer.LexemeNumber = 8;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.EchoKeyword;
                            OutputTable.Buffer.AttributeValue = "KEYWORD";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 109:
                            OutputTable.Buffer.Value = "ELSE";
                            OutputTable.Buffer.LexemeNumber = 9;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.EchoKeyword;
                            OutputTable.Buffer.AttributeValue = "KEYWORD";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 110:
                            OutputTable.Buffer.Value = "{";
                            OutputTable.Buffer.LexemeNumber = 10;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.LeftBrace;
                            OutputTable.Buffer.AttributeValue = "LPAR_S";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            flag = false;
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 111:
                            OutputTable.Buffer.Value = "}";
                            OutputTable.Buffer.LexemeNumber = 11;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.RightBrace;
                            OutputTable.Buffer.AttributeValue = "RPAR_S";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 112:
                            OutputTable.Buffer.Value = lexBuf;
                            OutputTable.Buffer.LexemeNumber = 12;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.Constant;
                            OutputTable.Buffer.AttributeValue = "CONSTANT";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 113:
                            OutputTable.Buffer.Value = " ";
                            OutputTable.Buffer.LexemeNumber = 13;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.Space;
                            OutputTable.Buffer.AttributeValue = "SPACE";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 114:
                            OutputTable.Buffer.Value = lexBuf;
                            OutputTable.Buffer.LexemeNumber = 14;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.Identifier;
                            OutputTable.Buffer.AttributeValue = "IDENTIFIER";
                            if (flag)
                            {
                                OutputTable.Buffer.AttributeValue = atrBuf;
                                OutputTable.Buffer.isIdentifier = true;
                            }
                            else
                                OutputTable.Buffer.isConditionalBranch = true;
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 115:
                            OutputTable.Buffer.Value = "+";
                            OutputTable.Buffer.LexemeNumber = 15;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.PlusOperation;
                            OutputTable.Buffer.AttributeValue = "OPERATION";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 116:
                            OutputTable.Buffer.Value = "-";
                            OutputTable.Buffer.LexemeNumber = 16;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.MinusOperation;
                            OutputTable.Buffer.AttributeValue = "OPERATION";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 117:
                            OutputTable.Buffer.Value = "=";
                            OutputTable.Buffer.LexemeNumber = 17;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.AssignOperation;
                            OutputTable.Buffer.AttributeValue = "ASSIGN";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 118:
                            OutputTable.Buffer.Value = lexBuf;
                            OutputTable.Buffer.LexemeNumber = 18;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.ComparsionOpearation;
                            OutputTable.Buffer.AttributeValue = "COMPARSION";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 119:
                            OutputTable.Buffer.Value = "/";
                            OutputTable.Buffer.LexemeNumber = 19;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.DivisionOperation;
                            OutputTable.Buffer.AttributeValue = "OPERATION";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 120:
                            OutputTable.Buffer.Value = "*";
                            OutputTable.Buffer.LexemeNumber = 20;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.MultipleOperation;
                            OutputTable.Buffer.AttributeValue = "OPERATION";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 121:
                            OutputTable.Buffer.Value = "%";
                            OutputTable.Buffer.LexemeNumber = 21;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.RemainderOfTheDivisionOperation;
                            OutputTable.Buffer.AttributeValue = "OPERATION";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 122:
                            OutputTable.Buffer.Value = ",";
                            OutputTable.Buffer.LexemeNumber = 22;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.Comma;
                            OutputTable.Buffer.AttributeValue = "COMMA";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 123:
                            OutputTable.Buffer.Value = ";";
                            OutputTable.Buffer.LexemeNumber = 23;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.Semicolon;
                            OutputTable.Buffer.AttributeValue = "SEMICOLON";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            if (flag)
                            {
                                atrBuf = "";
                                flag = false;
                            }
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 124:
                            OutputTable.Buffer.Value = "(";
                            OutputTable.Buffer.LexemeNumber = 24;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.LeftParentheses;
                            OutputTable.Buffer.AttributeValue = "LPAR_R";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 125:
                            OutputTable.Buffer.Value = ")";
                            OutputTable.Buffer.LexemeNumber = 25;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.RightParentheses;
                            OutputTable.Buffer.AttributeValue = "RPAR_R";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                        case 126:
                            OutputTable.Buffer.Value = "INT";
                            OutputTable.Buffer.LexemeNumber = 26;
                            OutputTable.Buffer.StringNumber = i;
                            OutputTable.Buffer.Token = TranslationToken.IntDataType;
                            OutputTable.Buffer.AttributeValue = "DIGIT";
                            OutputTable.Buffer.LineNumber = j - OutputTable.Buffer.Value.Count();
                            if (!flag)
                            {
                                atrBuf = "INT";
                                flag = true;
                            }
                            OutputTable.Put();
                            lexBuf = "";
                            _state = 0;
                            break;
                    }
                }
            }

            return OutputTable;
        }
    }
}
