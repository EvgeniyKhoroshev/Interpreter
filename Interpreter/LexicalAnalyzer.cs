using Interpreter.Extensions;
using Interpreter.Translation;
using System;
using System.Linq;
using Interpreter.Constants;
using System.Collections.Generic;
using Interpreter.TranslationResult;

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

        /// <summary>
        /// Lexical analysis state log.
        /// </summary>
        public List<string> StateLog = new List<string>();

        /// <summary>
        /// Pattern of unknown symbol error text.
        /// </summary>
        private readonly string _unknownSymbolErrorPattern = "Unknown symbol. String № {0}, символ № [{1}] = {2}";

        /// <summary>
        /// Could not find any lexeme error text pattern.
        /// </summary>
        private readonly string _couldNotFindAnyLexemePattern = "Could not find any lexeme. " +
                                                                "String №{0}, символ № [{1}] = {2}.";

        /// <summary>
        /// Pattern to print lexical analysis log.
        /// </summary>
        private readonly string _stateLogMessagePattern = "Value = {0} State = {1} \n";

        /// <summary>
        /// The value of state which defines that the symbol 
        /// is not supported by current language lexical rules.
        /// </summary>
        const int UnknownSybolStateValue = -1;

        public LexicalAnalyzer(string[] input)
        {
            _code = input;
            _translationTable = (int[,])FileExtensions.ReadFromFile(typeof(int[,]), PathConstants.LexicalTablePath);
            _rowKeys = (char[])FileExtensions.ReadFromFile(typeof(char[]), PathConstants.LexicalAnalyzerRows);
        }

        public List<string> _errors = new List<string>();
        private int GetRowByID(char id) // Получение строки по входному символу
        {
            if (id == '\t')
                return GetRowByID(' ');

            if (char.IsDigit(id))
                return 19;

            if (char.IsLetter(id))
            {
                var containsLetter = _rowKeys
                    .FirstOrDefault(x => string.Compare(id.ToString(), x.ToString(),
                                                StringComparison.InvariantCultureIgnoreCase) == 0);

                if (containsLetter == default)
                    return 18;
            }

            for (int i = 0; i < _rowKeys.Length; ++i)
                if (string.Compare(_rowKeys[i].ToString(), id.ToString(), 
                    comparisonType: StringComparison.InvariantCultureIgnoreCase) == default)
                    return i;
            
            return -1;
        }

        public TranslationTable TranslateCode() //Лексический анализ
        {
            var _outputTable = new TranslationTable(); //Выходная таблица трансляции;
            for (int i = 0; i < _code.Count(); ++i) // Перебор входных строк
            {
                var currentLine = _code[i];
                var currentEntryInfo = new CurrentEntryInfo();

                while (currentLine.Length > currentEntryInfo.SymbolNumber) // Перебор символов текущей строки
                {
                    var currentKey = GetRowByID(currentLine[currentEntryInfo.SymbolNumber]);

                    if (currentKey == UnknownSybolStateValue)
                    {
                        AddUnknownSybolStateError(i, currentLine, currentEntryInfo);

                        currentEntryInfo.CurrentState = 0;
                        ++currentEntryInfo.SymbolNumber;

                        continue;
                    }

                    SetCurrentState(currentEntryInfo, currentKey);
                    AddStateLogMessage(currentLine, currentEntryInfo);

                    if (currentEntryInfo.CurrentState == 0)
                    {
                        AddCouldNotFindAnyLexemeError(i, currentLine, currentEntryInfo);

                        currentEntryInfo.CurrentState = 0;
                        ++currentEntryInfo.SymbolNumber;

                        continue;
                    }

                    if (currentEntryInfo.CurrentState < 100)
                    {
                        currentEntryInfo.LexemeBuffer += currentLine[currentEntryInfo.SymbolNumber];
                        currentEntryInfo.CurrentState = _translationTable[currentKey, currentEntryInfo.CurrentState];

                        ++currentEntryInfo.SymbolNumber;

                        continue;
                    }

                    SetTokenValue(currentEntryInfo, _outputTable.Buffer);

                    _outputTable.InsertTranslationResult();
                    _outputTable.Buffer.Clear();
                    currentEntryInfo.Clear();
                    ++currentEntryInfo.SymbolNumber;
                }
            }

            return _outputTable;
        }

        private void SetCurrentState(CurrentEntryInfo currentEntryInfo, int currentKey)
        {
            currentEntryInfo.CurrentState = _translationTable[currentKey, currentEntryInfo.CurrentState];
        }

        private void AddStateLogMessage(string currentLine, CurrentEntryInfo currentEntryInfo)
        {
            StateLog.Add(string.Format(
                                    _stateLogMessagePattern,
                                    currentLine[currentEntryInfo.SymbolNumber],
                                    currentEntryInfo.CurrentState));
        }

        private void AddUnknownSybolStateError(
            int lineNumber, 
            string currentLine, 
            CurrentEntryInfo currentEntryInfo)
        {
            _errors.Add(string.Format(
                _unknownSymbolErrorPattern, 
                lineNumber,
                currentEntryInfo.SymbolNumber,
                currentLine[currentEntryInfo.SymbolNumber]));
        }

        private void AddCouldNotFindAnyLexemeError(
            int lineNumber,
            string currentLine,
            CurrentEntryInfo currentEntryInfo)
        {
            _errors.Add(string.Format(
                _couldNotFindAnyLexemePattern,
                (lineNumber + 1),
                (currentEntryInfo.SymbolNumber + 1),
                currentLine[currentEntryInfo.SymbolNumber]));
        }

        private void SetTokenValue(CurrentEntryInfo info, LexicalToken lexicalToken)
        {
            switch (info.CurrentState)
            {
                case 101:
                    lexicalToken.Token = TranslationToken.BooleanDataType;
                    break;
                case 102:
                    lexicalToken.Token = TranslationToken.BreakKeyword;
                    break;
                case 103:
                    lexicalToken.Token = TranslationToken.Constant;
                    break;
                case 104:
                    lexicalToken.Token = TranslationToken.Constant;
                    break;
                case 105:
                    lexicalToken.Token = TranslationToken.WhileKeyword;
                    break;
                case 106:
                    lexicalToken.Token = TranslationToken.InputKeyword;
                    break;
                case 107:
                    lexicalToken.Token = TranslationToken.IfKeyword;
                    break;
                case 108:
                    lexicalToken.Token = TranslationToken.EchoKeyword;
                    break;
                case 109:
                    lexicalToken.Token = TranslationToken.ElseKeyword;
                    break;
                case 110:
                    lexicalToken.Token = TranslationToken.LeftBrace;
                    break;
                case 111:                    
                    lexicalToken.Token = TranslationToken.RightBrace;
                    break;
                case 112:
                    lexicalToken.Token = TranslationToken.Constant;
                    break;
                case 113:
                    lexicalToken.Token = TranslationToken.Space;
                    break;
                case 114:
                    lexicalToken.Token = TranslationToken.Identifier;
                    break;
                case 115:
                    lexicalToken.Token = TranslationToken.PlusOperation;
                    break;
                case 116:                    
                    lexicalToken.Token = TranslationToken.MinusOperation;
                    break;
                case 117:
                    lexicalToken.Token = TranslationToken.AssignOperation;
                    break;
                case 118:               
                    lexicalToken.Token = TranslationToken.ComparsionOpearation;
                    break;
                case 119:
                    lexicalToken.Token = TranslationToken.DivisionOperation;
                    break;
                case 120:
                    lexicalToken.Token = TranslationToken.MultipleOperation;
                    break;
                case 121:
                    lexicalToken.Token = TranslationToken.RemainderOfTheDivisionOperation;
                    break;
                case 122:
                    lexicalToken.Token = TranslationToken.Comma;
                    break;
                case 123:
                    lexicalToken.Token = TranslationToken.Semicolon;
                    break;
                case 124:                    
                    lexicalToken.Token = TranslationToken.LeftParentheses;
                    break;
                case 125:                    
                    lexicalToken.Token = TranslationToken.RightParentheses;
                    break;
                case 126:
                    lexicalToken.Token = TranslationToken.Digit;
                    break;
            }

            lexicalToken.TokenAttributeValue = info.LexemeBuffer;
            lexicalToken.StringNumber = info.StringNumber;
            lexicalToken.ColumnIndex = info.SymbolNumber;
        }
    }



    public class CurrentEntryInfo
    {
        public int CurrentState { get; set; }
        public string LexemeBuffer { get; set; }
        public int StringNumber { get; set; }
        public int SymbolNumber { get; set; }
    }

    public static class CurrentEntryExtensions
    {
        public static void Clear(this CurrentEntryInfo _)
        {
            _.CurrentState = default;
            _.LexemeBuffer = default;
        }
    }
}
