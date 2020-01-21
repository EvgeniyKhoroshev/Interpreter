using Interpreter.Translation;
using Interpreter.TranslationResult;
using System;
using System.Collections.Generic;

namespace Interpreter
{
    internal class PostfixNotation
    {        
        /// <summary>
        /// Таблица приоритетов операций.
        /// </summary>
        private Dictionary<TranslationToken, int> _priorityTable;

        private LexicalToken Buffer = new LexicalToken();
        private LexicalToken bufForIO;
        private int markCounter = 0, currentOut = -1;                           // Счетчик меток; индикатор открытого цикла
        private bool subString = false;                                         // Флаг для разбора условий
        public TranslationTable source;                                         // Исходный класс для разбора

        

        public Queue<LexicalToken> output;                                  // Результат преобразования в инверсную запись
        private int priority = 0;                                               // Текущий приоритет операций
        public string out_log;                                                  // Лог для ошибок
        public TranslationTable regroupedTable = new TranslationTable();        // Класс для удаления ненужной информации из исходного класса
       
        public PostfixNotation()
        {
            SetUpPriorityTable();
        }
        /*------------------------------------------------------------------------------------------------------------------------------------------*/
        public void regroup()                                                   // Функция для удаления ненужной информации 
                                                                                //из исходного класса
        {
            bool flag = true;                                                   // Флаг для удаления обьявлений и имени программы
            while (source.TranslationList.Count > 0)                            // Основной цикл для прогона всех обектов
            {
                regroupedTable.Buffer = source.TranslationList.Dequeue();       // Извлекаем объект из класса-источника в буфер
                if (flag && (regroupedTable.Buffer.Token == TranslationToken.LeftBrace))               // Наличие флага и "{" говорит о необходимости 
                                                                                                       //пропустить имя программы и добавить "{"
                {
                    regroupedTable.InsertTranslationResult();
                    flag = false;
                    continue;
                }
                switch (regroupedTable.Buffer.Token)                             // Если поле "Token" соответствует одному из
                                                                                 //заданных - их необходимо пропустить
                {
                    case TranslationToken.BooleanDataType:
                    case TranslationToken.Digit:
                    case TranslationToken.Comma:
                        flag = true;
                        regroupedTable.Buffer.Clear();
                        continue;
                    default: break;
                }
                if (regroupedTable.Buffer.Token == TranslationToken.Semicolon)                         // Наличие ";" говорит о необходимости завершить
                                                                                                       //пропуск строки
                {
                    if (!flag)
                        regroupedTable.InsertTranslationResult();
                    flag = false;
                    regroupedTable.Buffer.Clear();
                    continue;
                }
                if (!flag && regroupedTable.Buffer.TokenAttributeValue != " ")                // Отсутствие флага пропуска говорит о том,
                                                                                //что текущий обьект можно поместить в результат,
                                                                                //если поле "Token" не равно " "
                    regroupedTable.InsertTranslationResult();
            }
        }
        public void postfixRecord()                                             // Функция преобразования в польскую инверсную нотацию
        {
            output = new Queue<LexicalToken>();                             // Инициализация переменной для результата
            Buffer.Clear();
            while (regroupedTable.TranslationList.Count > 0)
            {
                Buffer = regroupedTable.TranslationList.Dequeue();
                casesOfTransactions();
            }
        }

        private void casesOfTransactions()                                              // Функция, обрабатывающая варианты действий
                                                                                        //в соответствии с пришедшим "Token"
        {
            switch (Buffer.Token)
            {
                case TranslationToken.Identifier:
                    postfixString();
                    break;
                case TranslationToken.BreakKeyword:
                    if (currentOut != -1)
                    {
                        createTransition(currentOut, false);
                        output.Enqueue(Buffer);
                    }
                    else
                        out_log += "Строка [" + Convert.ToString(Buffer.StringNumber + 1) + "] оператор Break используется вне цикла.";
                    break;
                case TranslationToken.EchoKeyword:
                    bufForIO = Buffer;
                    Buffer = regroupedTable.TranslationList.Dequeue();
                    postfixString();
                    output.Enqueue(bufForIO);
                    break;
                case TranslationToken.WhileKeyword:
                    postfixWhile();
                    break;
                case TranslationToken.InputKeyword:
                    bufForIO = Buffer;
                    output.Enqueue(regroupedTable.TranslationList.Dequeue());
                    output.Enqueue(regroupedTable.TranslationList.Dequeue());
                    output.Enqueue(bufForIO);

                    break;
                case TranslationToken.IfKeyword:
                    postfixIf();
                    break;
                default:
                    break;
            }
        }

        private void postfixWhile()                                                     // Постфиксная запись оператора "WHILE"
        {
            int loopIn = markCounter, loopOut = markCounter + 1;
            markCounter += 2;
            subString = true;

            // Добавляем метку перед условием
            createMark(loopIn);
            output.Enqueue(Buffer);

            // Проверяем условие
            Buffer.Clear();
            Buffer = regroupedTable.TranslationList.Dequeue();
            postfixString();

            // Добавляем условный переход на выход из цикла
            createTransition(loopOut, true);
            output.Enqueue(Buffer);
            Buffer.Clear();
            Buffer = regroupedTable.TranslationList.Dequeue();
            currentOut = loopOut;
            if (Buffer.Token == TranslationToken.LeftBrace)
            {
                Buffer.Clear();
                while (Buffer.Token != TranslationToken.RightBrace)
                {
                    Buffer = regroupedTable.TranslationList.Dequeue();
                    casesOfTransactions();
                }
                createTransition(loopIn, false);
                output.Enqueue(Buffer);
                createMark(loopOut);
                output.Enqueue(Buffer);
            }
            else
            {
                casesOfTransactions();
                createTransition(loopIn, false);
                output.Enqueue(Buffer);
                createMark(loopOut);
                output.Enqueue(Buffer);
            }
            currentOut = -1;
        }

        private void postfixIf()                                                        // Постфиксная запись оператора "IF"
        {
            int conditionOut = markCounter, elseOut = markCounter + 1;
            markCounter += 2;
            subString = true;
            // Проверяем условие
            Buffer.Clear();
            Buffer = regroupedTable.TranslationList.Dequeue();
            postfixString();
            // Добавляем условный переход для выхода
            createTransition(conditionOut, true);
            output.Enqueue(Buffer);
            Buffer.Clear();
            // Транслируем множественные/одиночные выражения цикла
            Buffer = regroupedTable.TranslationList.Dequeue();
            if (Buffer.Token == TranslationToken.LeftBrace)
            {
                Buffer.Clear();
                while (Buffer.Token != TranslationToken.RightBrace)
                {
                    Buffer = regroupedTable.TranslationList.Dequeue();
                    casesOfTransactions();
                }
                createTransition(elseOut, false);
                output.Enqueue(Buffer);
                createMark(conditionOut);
                output.Enqueue(Buffer);
            }
            else
            {
                casesOfTransactions();
                createTransition(elseOut, false);
                output.Enqueue(Buffer);
                createMark(conditionOut);
                output.Enqueue(Buffer);
            }
            if (regroupedTable.TranslationList.Peek().Token == TranslationToken.EchoKeyword)
            {
                regroupedTable.TranslationList.Dequeue();
                Buffer = regroupedTable.TranslationList.Dequeue();
                if (Buffer.Token == TranslationToken.LeftBrace)
                {
                    Buffer.Clear();
                    while (Buffer.Token != TranslationToken.RightBrace)
                    {
                        Buffer = regroupedTable.TranslationList.Dequeue();
                        casesOfTransactions();
                    }
                    createMark(elseOut);
                    output.Enqueue(Buffer);
                }
                else
                {
                    casesOfTransactions();
                    createMark(elseOut);
                    output.Enqueue(Buffer);
                }

            }
            else
            {
                createMark(elseOut);
                output.Enqueue(Buffer);
            }

        }

        private void createMark(int mark)                                               // Функция создания n-ой метки
        {
            Buffer.Clear();
            Buffer.Token = TranslationToken.GotoLabel;
            Buffer.TokenAttributeValue = "Label " + Convert.ToString(mark) + ":";
            Buffer.TokenAttributeValue = "LABEL";
            Buffer.ColumnIndex = 0;
            Buffer.StringNumber = mark;
        }

        private void createTransition(int mark, bool conditional)                       // Функция создания условного/безусловного перехода к метке
        {
            Buffer.Clear();
            Buffer.Token = TranslationToken.GotoTransition;
            Buffer.TokenAttributeValue = "goto " + Convert.ToString(mark);
            Buffer.TokenAttributeValue = "GOTO";
            Buffer.ColumnIndex = 0;
            Buffer.StringNumber = mark;
            //Buffer.isConditionalBranch = conditional;
        }

        private void postfixString()                                                    // Трансляция выражений
        {
            Stack<LexicalToken> workStack = new Stack<LexicalToken>();
            priority = 0;
            while (true)
            {
                // Проверка на некорректность данных.
                if (Buffer.Token == TranslationToken.LeftBrace || Buffer.Token == TranslationToken.RightBrace ||
                    Buffer.Token == TranslationToken.Semicolon || Buffer.Token == TranslationToken.WhileKeyword ||
                    Buffer.Token == TranslationToken.InputKeyword || Buffer.Token == TranslationToken.InputKeyword ||
                    Buffer.Token == TranslationToken.EchoKeyword)
                    break;
                if (Buffer.Token == TranslationToken.LeftParentheses)
                {
                    priority = _priorityTable[Buffer.Token];
                    workStack.Push(Buffer);
                    Buffer.Clear();
                    Buffer = regroupedTable.TranslationList.Dequeue();
                    continue;
                }
                // Обработка скобок
                if (Buffer.Token == TranslationToken.RightParentheses)
                {
                    while (workStack.Count > 0 && workStack.Peek().Token != TranslationToken.LeftParentheses)
                        output.Enqueue(workStack.Pop());
                    if (workStack.Count > 0)
                    {
                        workStack.Pop();
                        if (workStack.Count > 0)
                            priority = _priorityTable[workStack.Peek().Token];
                        else if (subString)
                        {
                            subString = false;
                            break;
                        }
                        priority = 0;

                    }
                    Buffer = regroupedTable.TranslationList.Dequeue();
                    continue;
                }
                if (Buffer.Token == TranslationToken.Identifier || Buffer.Token == TranslationToken.Constant)
                {
                    output.Enqueue(Buffer);
                    Buffer.Clear();
                    Buffer = regroupedTable.TranslationList.Dequeue();
                    continue;
                }
                if (_priorityTable.ContainsKey(Buffer.Token))
                    if (priority < _priorityTable[Buffer.Token])
                    {
                        priority = _priorityTable[Buffer.Token];
                        workStack.Push(Buffer);
                        Buffer.Clear();
                        Buffer = regroupedTable.TranslationList.Dequeue();
                        continue;
                    }
                    else
                    {
                        while (true)
                        {
                            if (workStack.Count > 0 && _priorityTable[Buffer.Token] <= _priorityTable[workStack.Peek().Token])
                                output.Enqueue(workStack.Pop());
                            else
                            {
                                workStack.Push(Buffer);
                                Buffer = regroupedTable.TranslationList.Dequeue();
                                break;
                            }
                        }
                    }
            }
            while (workStack.Count > 0)
                if (workStack.Peek().Token == TranslationToken.RightParentheses || workStack.Peek().Token == TranslationToken.LeftParentheses)
                    workStack.Pop();
                else
                    output.Enqueue(workStack.Pop());
            if (Buffer.Token != TranslationToken.RightParentheses && Buffer.Token != TranslationToken.LeftParentheses && Buffer.Token != TranslationToken.RightBrace && Buffer.Token != TranslationToken.LeftBrace)
                output.Enqueue(Buffer);
        }

        private void SetUpPriorityTable()
        {
            _priorityTable = new Dictionary<TranslationToken, int>
            {
                { TranslationToken.Semicolon, 0 },
                { TranslationToken.AssignOperation, 1 },
                { TranslationToken.LeftParentheses, 2 },
                { TranslationToken.RightParentheses, 2 },
                { TranslationToken.LeftBrace, 2 },
                { TranslationToken.RightBrace, 2 },
                { TranslationToken.ComparsionOpearation, 3 },
                { TranslationToken.PlusOperation, 4 },
                { TranslationToken.MinusOperation, 4 },
                { TranslationToken.RemainderOfTheDivisionOperation, 5 },
                { TranslationToken.MultipleOperation, 6 },
                { TranslationToken.DivisionOperation, 6 }
            };
        }
    }
}
