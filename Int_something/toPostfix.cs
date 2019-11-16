using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int_something
{
    class PostfixNotation : LL1LexilacAnalyzer
    {
        TranslatedToken bufForIO;
        private int markCounter = 0, currentOut = -1;                           // Счетчик меток; индикатор открытого цикла
        private bool subString = false;                                         // Флаг для разбора условий
        public LL1LexilacAnalyzer source;                                         // Исходный класс для разбора
        private Dictionary<char, int> hashTable = new Dictionary<char, int>();  // Таблица приоритетов операций
        public Queue<TranslatedToken> output;                                  // Результат преобразования в инверсную запись
        private int priority = 0;                                               // Текущий приоритет операций
        public string out_log;                                                  // Лог для ошибок
        public LL1LexilacAnalyzer regroupedTable = new LL1LexilacAnalyzer();        // Класс для удаления ненужной информации из исходного класса
/*------------------------------------------------------------------------------------------------------------------------------------------*/
        public void regroup()                                                   // Функция для удаления ненужной информации 
                                                                                //из исходного класса
        {
            bool flag = true;                                                   // Флаг для удаления обьявлений и имени программы
            while (source.TranslationList.Count > 0)                            // Основной цикл для прогона всех обектов
            {
                regroupedTable.Buffer = source.TranslationList.Dequeue();       // Извлекаем объект из класса-источника в буфер
                if (flag && (regroupedTable.Buffer.Token == '{'))               // Наличие флага и "{" говорит о необходимости 
                                                                                //пропустить имя программы и добавить "{"
                {
                    regroupedTable.Put();
                    flag = false;
                    continue;
                }
                switch(regroupedTable.Buffer.Token)                             // Если поле "Token" соответствует одному из
                                                                                //заданных - их необходимо пропустить
                {
                    case 'B': case 'd': case ',':
                        flag = true;
                        regroupedTable.ClearBuffer();
                        continue;
                    default: break;
                }
                if (regroupedTable.Buffer.Token == ';')                         // Наличие ";" говорит о необходимости завершить
                                                                                //пропуск строки
                {
                    if (!flag)
                        regroupedTable.Put();
                    flag = false;
                    regroupedTable.ClearBuffer();
                    continue;
                }
                if (!flag && regroupedTable.Buffer.Value != " ")                // Отсутствие флага пропуска говорит о том,
                                                                                //что текущий обьект можно поместить в результат,
                                                                                //если поле "Token" не равно " "
                    regroupedTable.Put();
            }
        }
        public void postfixRecord()                                             // Функция преобразования в польскую инверсную нотацию
        {
            hashInit();
            output = new Queue<TranslatedToken>();                             // Инициализация переменной для результата
            this.ClearBuffer();
            while(regroupedTable.TranslationList.Count > 0)
            {
                this.Buffer = regroupedTable.TranslationList.Dequeue();
                casesOfTransactions();
            }
        }
        void casesOfTransactions()                                              // Функция, обрабатывающая варианты действий
                                                                                //в соответствии с пришедшим "Token"
        {
            switch (this.Buffer.Token)
            {
                case 'X':
                    postfixString();
                    break;
                case 'R':
                    if (currentOut != -1)
                    {
                        createTransition(currentOut, false);
                        output.Enqueue(this.Buffer);
                    }
                    else
                        out_log += "Строка [" + Convert.ToString(this.Buffer.StringNumber + 1) + "] оператор Break используется вне цикла.";
                    break;
                case 'e':
                    bufForIO = this.Buffer;
                    this.Buffer = regroupedTable.TranslationList.Dequeue();
                    postfixString();
                    output.Enqueue(bufForIO);
                    break;
                case 'W':
                    postfixWhile();
                    break;
                case 'i':
                    bufForIO = this.Buffer;
                    output.Enqueue(regroupedTable.TranslationList.Dequeue());
                    output.Enqueue(regroupedTable.TranslationList.Dequeue());
                    output.Enqueue(bufForIO);

                    break;
                case 'I':
                    postfixIf();
                    break;
                default:
                    break;
            }
        }
        void postfixWhile()                                                     // Постфиксная запись оператора "WHILE"
        {
            int loopIn = markCounter, loopOut = markCounter+1;
            markCounter += 2;
            subString = true;

            // Добавляем метку перед условием
            createMark(loopIn);
            output.Enqueue(this.Buffer);

            // Проверяем условие
            this.ClearBuffer();
            this.Buffer = regroupedTable.TranslationList.Dequeue();
            postfixString();
        
            // Добавляем условный переход на выход из цикла
            createTransition(loopOut, true);
            output.Enqueue(this.Buffer);
            this.ClearBuffer();
            this.Buffer = regroupedTable.TranslationList.Dequeue();
            currentOut = loopOut;
            if (this.Buffer.Token == '{')
            {
                this.ClearBuffer();
                while (this.Buffer.Token != '}')
                {
                    this.Buffer = regroupedTable.TranslationList.Dequeue();
                    casesOfTransactions();
                }
                createTransition(loopIn, false);
                output.Enqueue(this.Buffer);
                createMark(loopOut);
                output.Enqueue(this.Buffer);
            }
            else
            {
                casesOfTransactions();
                createTransition(loopIn, false);
                output.Enqueue(this.Buffer);
                createMark(loopOut);
                output.Enqueue(this.Buffer);
            }
            currentOut = -1;
        }
        void postfixIf()                                                        // Постфиксная запись оператора "IF"
        {
            int conditionOut = markCounter, elseOut = markCounter+1;
            markCounter += 2;
            subString = true;
            // Проверяем условие
            this.ClearBuffer();
            this.Buffer = regroupedTable.TranslationList.Dequeue();
            postfixString();
            // Добавляем условный переход для выхода
            createTransition(conditionOut, true);
            output.Enqueue(this.Buffer);
            this.ClearBuffer();
            // Транслируем множественные/одиночные выражения цикла
            this.Buffer = regroupedTable.TranslationList.Dequeue();
            if (this.Buffer.Token == '{')
            {
                this.ClearBuffer();
                while (this.Buffer.Token != '}')
                {
                    this.Buffer = regroupedTable.TranslationList.Dequeue();
                    casesOfTransactions();
                }
                createTransition(elseOut, false);
                output.Enqueue(this.Buffer);
                createMark(conditionOut);
                output.Enqueue(this.Buffer);
            }
            else
            {
                casesOfTransactions();
                createTransition(elseOut, false);
                output.Enqueue(this.Buffer);
                createMark(conditionOut);
                output.Enqueue(this.Buffer);
            }
            if (regroupedTable.TranslationList.Peek().Token == 'E')
            {
                regroupedTable.TranslationList.Dequeue();
                this.Buffer = regroupedTable.TranslationList.Dequeue();
                if (this.Buffer.Token == '{')
                {
                    this.ClearBuffer();
                    while (this.Buffer.Token != '}')
                    {
                        this.Buffer = regroupedTable.TranslationList.Dequeue();
                        casesOfTransactions();
                    }
                    createMark(elseOut);
                    output.Enqueue(this.Buffer);
                }
                else
                {
                    casesOfTransactions();
                    createMark(elseOut);
                    output.Enqueue(this.Buffer);
                }

            }
            else
            {
                createMark(elseOut);
                output.Enqueue(this.Buffer);
            }

        }
        void createMark(int mark)                                               // Функция создания n-ой метки
        {
            this.ClearBuffer();
            this.Buffer.Token = ':';
            this.Buffer.Value = "Label " + Convert.ToString(mark)+":";
            this.Buffer.AttributeValue = "LABEL";
            this.Buffer.Lexeme = 100;
            this.Buffer.numberInProgram = 0;
            this.Buffer.StringNumber = mark;
        }
        void createTransition(int mark, bool conditional)                       // Функция создания условного/безусловного перехода к метке
        {
            this.ClearBuffer();
            this.Buffer.Token = '>';
            this.Buffer.Value = "goto " + Convert.ToString(mark);
            this.Buffer.AttributeValue = "GOTO";
            this.Buffer.Lexeme = 200;
            this.Buffer.numberInProgram = 0;
            this.Buffer.StringNumber = mark;
            this.Buffer.isConditionalBranch = conditional;
        }
        void postfixString()                                                    // Трансляция выражений
        {
            Stack<TranslatedToken> workStack = new Stack<TranslatedToken>();
            priority = 0;
            while (true)
            {
                // Проверка на некорректность данных.
                if (this.Buffer.Token == '{' || this.Buffer.Token == '}' ||
                    this.Buffer.Token == ';' || this.Buffer.Token == 'W' ||
                    this.Buffer.Token == 'I' || this.Buffer.Token == 'i' ||
                    this.Buffer.Token == 'e')
                    break;
                if (this.Buffer.Token == '(')
                {
                    priority = hashTable[this.Buffer.Token];
                    workStack.Push(this.Buffer);
                    this.ClearBuffer();
                    this.Buffer = regroupedTable.TranslationList.Dequeue();
                    continue;
                }
                // Обработка скобок
                if (this.Buffer.Token == ')')
                {
                    while (workStack.Count > 0 && workStack.Peek().Token != '(')
                        output.Enqueue(workStack.Pop());
                    if (workStack.Count > 0)
                    {
                        workStack.Pop();
                        if (workStack.Count > 0)
                            priority = hashTable[workStack.Peek().Token];
                        else if (subString)
                        {
                            subString = false;
                            break;
                        }
                        priority = 0;

                    }
                    this.Buffer = regroupedTable.TranslationList.Dequeue();
                    continue;
                }
                if (this.Buffer.Token == 'X' || this.Buffer.Token == 'C')
                {
                    output.Enqueue(this.Buffer);
                    this.ClearBuffer();
                    this.Buffer = regroupedTable.TranslationList.Dequeue();
                    continue;
                }
                if (hashTable.ContainsKey(this.Buffer.Token))
                    if (priority < hashTable[this.Buffer.Token])
                    {
                        priority = hashTable[this.Buffer.Token];
                        workStack.Push(this.Buffer);
                        this.ClearBuffer();
                        this.Buffer = regroupedTable.TranslationList.Dequeue();
                        continue;
                    }
                    else
                    {
                        while (true)
                        {
                            if (workStack.Count > 0 && hashTable[this.Buffer.Token] <= hashTable[workStack.Peek().Token])
                                output.Enqueue(workStack.Pop());
                            else
                            {
                                workStack.Push(this.Buffer);
                                this.Buffer = regroupedTable.TranslationList.Dequeue();
                                break;
                            }
                        }
                    }
            }
            while (workStack.Count > 0)
                if (workStack.Peek().Token == ')' || workStack.Peek().Token == '(')
                    workStack.Pop();
                else
                    output.Enqueue(workStack.Pop());
            if (this.Buffer.Token != ')' && this.Buffer.Token != '(' && this.Buffer.Token != '}' && this.Buffer.Token != '{')
                output.Enqueue(this.Buffer);
        }
        void hashInit()                                                         // Инициализация хэш-таблицы приоритетов
        {
            hashTable.Add(';', 0);
            hashTable.Add('=', 1);
            hashTable.Add('(', 2);
            hashTable.Add(')', 2);
            hashTable.Add('}', 2);
            hashTable.Add('{', 2);
            hashTable.Add('c', 3);
            hashTable.Add('+', 4);
            hashTable.Add('-', 4);
            hashTable.Add('%', 5);
            hashTable.Add('*', 6);
            hashTable.Add('/', 6);

        }
    }
}
