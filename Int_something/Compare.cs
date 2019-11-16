using Int_something.TranslationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int_something
{
    class Compare
    {
        public int currentScore, problemCount;
        double fullScore;
        public Compare()
        {
            buffer = new execute_all();
            initializeBasic();
            problemCount = tasks.Count();
            currentTask = tasks.Dequeue();
            currentProblem = problemsToOut.Dequeue();
        }
        execute_all  input, buffer;
        public struct Problem
        {
            public int problemIndex;
            public string problem_annotation;
            public string problemText;
            public bool problemImage;
        }
        struct Task
        {
            public int triadCount;
            public LexicalToken basicEcho;
            public LexicalToken basicInput;
            public LexicalToken basicID;
            public Triad.Triada basicLoop;
        }
        public Queue<Problem> problemsToOut = new Queue<Problem>();
        public Problem currentProblem;
        Queue<Task> tasks = new Queue<Task>();
        Task currentTask;
        public string getResult(execute_all i)
        {
            
            input = i;
            return compare();
        }
        public double getFinalResult()
        {
            return fullScore/problemCount;
        }
        public void addCurrentResult()
        {
            fullScore += currentScore;
            currentScore = 0;
        }
        string compare()
        {
            string result = "";
            int score = 0;
            int index = 0;
            index = findEcho();
            if (currentTask.basicEcho.AttributeValue == "")
                score += 10;
            else
            if (index == -1)
            {
                result += "Ответ не выведен.\n";
            }
            else
            {
                input.takeOp(input.ThreeAddressCode[index].FirstOperand);
                if (input.currentInt.value == Convert.ToInt32(currentTask.basicEcho.Value))
                {
                    result += "Выведен верный ответ.\n";
                    score += 10;
                }
                if (input.currentInt.name == currentTask.basicEcho.AttributeValue)
                {
                    result += "Выведена верная переменная.\n";
                    score += 10;
                }
            }
            if (input.ThreeAddressCode.Count() + 1 >= currentTask.triadCount && input.ThreeAddressCode.Count() - 1 <= currentTask.triadCount)
            {
                score += 20;
            }
            else
            if (input.ThreeAddressCode.Count() + 2 > currentTask.triadCount)
            {
                score += 10;
                result += "Необходимо четко выполнять действия в соответствии с алгоритмом, представленным на рисунке.\n";
            }
            else
            if (input.ThreeAddressCode.Count() - 1 < currentTask.triadCount)
            {
                result += "Данную программу можно выполнить за меньшее количество действий. Внимательней читайте задание.\n";
                score += 5;
            }

            index = findInput();
            if (currentTask.basicInput.AttributeValue == "")
            {
                score += 10;
            }
            else
            if (index == -1)
            {
                result += "В условии задано ввести переменную " + currentTask.basicInput.AttributeValue + " с клавиатуры.\n";
            }
            else
            {
                input.takeOp(input.ThreeAddressCode[index].FirstOperand);
                if (input.currentInt.name == currentTask.basicInput.AttributeValue)
                {
                    score += 10;
                }
                else
                {
                    score += 5;
                }
            }
            if (input.source.Identifiers.isIdentifierExists(currentTask.basicID))
            {
                score += 20;
                if (input.source.Identifiers.intTable.ContainsKey(currentTask.basicID.Value))
                {
                    score += 15;
                    if (currentTask.basicID.AttributeValue == "")
                        score += 15;
                    else
                    if (input.source.Identifiers.intTable[currentTask.basicID.Value].value == Convert.ToInt32(currentTask.basicID.AttributeValue))
                        score += 15;
                }
            }
            else
            {
                result += "Заданная условием переменная " + currentTask.basicID.Value + " не найдена в программе.\n";
            }
            result += "Программа выполнена на " + score.ToString() + "% в соответствии с условием.";
            if (currentScore < score)
                currentScore = score;
            return result;
        }
        public void initializeBasic()
        {
            clearTask();
            currentTask.basicEcho.Value = "50";
            currentTask.basicEcho.AttributeValue = "A";
            currentTask.triadCount = 2;
            currentTask.basicID.Value = "A";
            currentTask.basicID.Token = 'X';
            currentTask.basicID.AttributeValue = "50";
            currentTask.basicInput.AttributeValue = "";
            tasks.Enqueue(currentTask);
            currentProblem.problemImage = true;
            currentProblem.problemIndex = 1;
            currentProblem.problemText = "Выполните действия, заданные алгоритмом на рисунке 1.\nДля вывода переменной используйте оператор 'echo <выражение>;' ";
            currentProblem.problem_annotation = "Рисунок 1. Вывод переменной A.";
            problemsToOut.Enqueue(currentProblem);

            clearTask();
            currentTask.basicEcho.Value = "1";
            currentTask.basicEcho.AttributeValue = "B";
            currentTask.triadCount = 5;
            currentTask.basicID.Value = "A";
            currentTask.basicID.Token = 'X';

            currentTask.basicInput.AttributeValue = "A";
            tasks.Enqueue(currentTask);
            currentProblem.problemImage = true;
            currentProblem.problemIndex = 2;
            currentProblem.problemText = "Выполните действия, заданные алгоритмом на рисунке 2. Переменную А введите с клавиатуры. Значение введенной переменной должно быть равно 6.";
            currentProblem.problem_annotation = "Рисунок 2. Вывод переменной B.";
            problemsToOut.Enqueue(currentProblem);
            clearTask();

            currentTask.basicEcho.Value = "120";
            currentTask.basicEcho.AttributeValue = "RESULT";
            currentTask.triadCount = 18;
            currentTask.basicID.Value = "COUNTER";
            currentTask.basicID.Token = 'X';

            currentTask.basicInput.AttributeValue = "FACT";
            tasks.Enqueue(currentTask);
            currentProblem.problemImage = true;
            currentProblem.problemIndex = 3;
            currentProblem.problemText = "Напишите программу, вычисляющую факториал числа 5 (5!). Определите, необходимые для работы алгоритма, переменные самостоятельно. Алгоритм описан на рисунке 3.\nПеременную fact введите с клавиатуры.";
            currentProblem.problem_annotation = "Рисунок 3. Вывод переменной B.";
            problemsToOut.Enqueue(currentProblem);
            clearTask();

        }
        void clearTask()
        {
            buffer.ClearBuffer();
            currentTask.basicEcho = buffer.Buffer;
            currentTask.basicID = buffer.Buffer;
            currentTask.basicInput = buffer.Buffer;
            buffer.clearBuf();
            currentTask.basicLoop =  buffer.BufferTriada;

            currentProblem.problemImage = false;
            currentProblem.problemText = "";
            currentProblem.problemIndex = 0;
            currentProblem.problem_annotation = "";
        }
        int findInput()
        {
            for (int i = 0; i < input.ThreeAddressCode.Count(); ++i)
                if (input.ThreeAddressCode[i].Operation.Token == 'i')
                    return i;
            return -1;
        }
        int findEcho()
        {
            for (int i = input.ThreeAddressCode.Count() - 1; i > 0; --i)
                if (input.ThreeAddressCode[i].Operation.Token == 'e')
                    return i;
            return -1;
        }

        public bool getNextProblem()
        {
            if (tasks.Count > 0 && problemsToOut.Count > 0)
            {
                currentTask = tasks.Dequeue();
                currentProblem = problemsToOut.Dequeue();
                return true;
            }
            return false;
        }

        internal execute_all execute_all
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal execute_all execute_all1
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
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
    }

}