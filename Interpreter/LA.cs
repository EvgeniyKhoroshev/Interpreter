using Interpreter.Translation;
using System;
using System.Collections;
using System.Linq;

namespace Interpreter
{
    public class LA
    {

        public static int[,] TransferTable =
//         0   1   2   3   4   5   6   7   8   9   10  11  12  13  14  15  16  17  18  19  20  21  22  23  24  25  26  27  28  29  30  31  32  33  34  35  36  37  38  39  40  41  42  43  44  45  46  47  48  49  50  51  52  53  54  55  56  57  58  59  60  61  62  63  64  65  66  67  68  
{
{   1,  40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   40, 2,  3,  40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 32, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   40, 40, 40, 4,  40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 15, 40, 40, 40, 40, 40, 21, 40, 40, 40, 40, 40, 40, 40, 40, 33, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   23, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 20, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   40, 5,  40, 40, 40, 40, 40, 40, 40, 10, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   29, 40, 40, 40, 40, 6,  40, 40, 40, 40, 40, 12, 40, 40, 40, 40, 17, 40, 40, 40, 40, 22, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 35, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   40, 40, 40, 40, 40, 40, 7,  40, 40, 40, 40, 40, 40, 14, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   40, 40, 40, 40, 40, 40, 40, 8,  40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   9,  40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 54, 40, 27, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 11, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 26, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   13, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 28, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 16, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 34, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 30, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 19, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 31, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   18, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,     40  },
{   40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 24, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 25, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    0,  113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   38, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 110,    111,    38, 113,    40, 115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    40  },
{   41, 114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  0,  114,    114,    114,    0,  114,    114,    0,  110,    111,    112,     113,    114,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  125,    0,  0,  0   },
{   42, 114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  0,  114,    114,    114,    0,  114,    114,    0,  110,    111,    112,     113,    114,    0,  0,  0,  0,  0,  0,  0,  0,  0,  124,    125,    0,  0,  0   },
{   46, 114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  0,  114,    114,    114,    0,  114,    114,    0,  110,    111,    112,    113,    114,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  125,    0,  0,  0   },
{   43, 114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    103,    114,    114,    114,    114,    104,    114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  0,  114,    114,    114,    0,  114,    114,    0,  110,    111,    112,    113,    114,    0,  0,  44, 0,  0,  0,  0,  0,  0,  0,  125,    0,  0,  0   },
{   52, 114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  0,  114,    114,    114,    0,  114,    114,    0,  110,    111,    112,    113,    114,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  125,    0,  0,  0   },
{   44, 114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  0,  114,    114,    114,    0,  114,    114,    0,  110,    111,    112,    113,    114,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  125,    53, 0,  0   },
{   50, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  105,    0,  0,  0,  0,  0,  107,    0,  0,  0,  108,    0,  0,  0,  110,    111,    0,  113,    0,  115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    0,  118,    118,    0   },
{   51, 114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    103,    114,    114,    114,    114,    104,    114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  0,  114,    114,    114,    0,  114,    114,    0,  110,    111,    112,    113,    114,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  125,    0,  0,  0   },
{   49, 114,    114,    114,    0,  114,    114,    114,    102,    114,    114,    114,    103,    114,    114,    114,    114,    104,    114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  0,  114,    114,    114,    0,  114,    114,    0,  110,    111,    112,    113,    114,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  125,    0,  0,  0   },
{   48, 114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  0,  114,    114,    114,    0,  114,    114,    0,  110,    111,    112,    113,    114,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   45, 114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  0,  114,    114,    114,    0,  114,    114,    0,  110,    111,    112,    113,    114,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  125,    0,  0,  0   },
{   47, 114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  0,  114,    114,    114,    0,  114,    114,    0,  110,    111,    112,    113,    114,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  125,    0,  0,  0   },
{   39, 114,    114,    114,    101,    114,    114,    114,    102,    114,    114,    114,    103,    114,    114,    114,    114,    104,    114,    114,    114,    114,    105,    114,    114,    114,    114,    106,    107,    114,    114,    114,    108,    114,    114,    109,    110,    111,    112,    39, 114,    115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    125,    118,    118,    126 },
{   36, 114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  0,  114,    114,    114,    0,  114,    114,    109,    110,    111,    0,  113,    114,    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0   },
{   37, 114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  114,    114,    114,    114,    0,  0,  114,    114,    114,    0,  114,    114,    0,  110,    111,    0,  113,    114,    0,  0,  0,  0,  0,  0,  0,  0,  123,    0,  125,    0,  0,  0   },
{   39, 114,    114,    114,    101,    114,    114,    114,    102,    114,    114,    114,    103,    114,    114,    114,    114,    104,    114,    114,    114,    114,    105,    114,    114,    114,    114,    106,    107,    114,    114,    114,    108,    114,    114,    109,    110,    111,    112,    113, 114,    115,    116,    117,    118,    119,    120,    121,    122,    123,    124,    125,    118,    118,    126 },
    };
        // Вектор терминальных символов
        private char[] Rows =
            {
                                'B',    'O',    'L',    'I',    'R',    'E',
                                'A',    'K',    'T',    'U',    'F',    'S',
                                'C',    'H',    'S',    'W',    'N',    'P',
                                '0',    '0',    '+',    '-',    '*',    '=',
                                '<',    ':',    '(',    ')',    ';',    ',',
                                '/',    '%',    ' ',    '{',    '}' ,   '$'

            };

        private readonly string[] _code;
        public LA(string[] input)
        {
            _code = input;
        }
        private int State; //Переменная состояния
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
                if (!Rows.Contains(buf))
                    return 18;
            }


            buf = char.ToUpper(id);
            for (int i = 0; i < Rows.Count(); ++i)
            {
                if (Rows[i] == buf)
                    return i;
            }
            return -1;
        }
        public TranslationTable TranslateCode() //Лексический анализ
        {
            stateLogLA = "Лог состояний: \n";
            TranslationTable OutputTable = new TranslationTable(); //Выходная таблица трансляции;
            id_Table identifiers = new id_Table();
            State = 0;
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
                        State = 0;
                        ++j;
                        continue;
                    }
                    Switcher = TransferTable[RowCounter, State];
                    stateLogLA += Convert.ToString(Switcher) + ' ';
                    if (Switcher == 0)
                    {
                        ErrorListLA.Enqueue("Не удается обнаружить лексему в строке №" + Convert.ToString(i + 1) + " символ № [" + Convert.ToString(j + 1) + "] :" + buf[j]);
                        ++ErrorCounter;
                        State = 0;
                        ++j;
                        continue;
                    }
                    if (Switcher < 100)
                    {
                        if (char.IsLetter(buf[j]))
                            lexBuf += char.ToUpper(buf[j]);
                        else
                            lexBuf += buf[j];
                        State = TransferTable[RowCounter, State];
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
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
                            State = 0;
                            break;
                    }
                }
            }

            return OutputTable;
        }
    }
}
