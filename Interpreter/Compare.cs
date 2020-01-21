using Interpreter.Translation;
using Interpreter.TranslationResult;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interpreter
{
    internal class Compare
    {
        private Triada TriadaBuffer = new Triada();
        private LexicalToken Buffer = new LexicalToken();
        public int currentScore, problemCount;
        private double fullScore;
        public Compare()
        {
            InitializeBasic();
            problemCount = tasks.Count();
            currentTask = tasks.Dequeue();
            currentProblem = problemsToOut.Dequeue();
        }

        private execute_all input;
        public struct Problem
        {
            public int problemIndex;
            public string problem_annotation;
            public string problemText;
            public bool problemImage;
        }

        private struct Task
        {
            public int triadCount;
            public LexicalToken basicEcho;
            public LexicalToken basicInput;
            public LexicalToken basicID;
            public Triada basicLoop;
        }
        public Queue<Problem> problemsToOut = new Queue<Problem>();
        public Problem currentProblem;
        private Queue<Task> tasks = new Queue<Task>();
        private Task currentTask;
        public string getResult(execute_all i)
        {

            input = i;
            return compare();
        }
        public double getFinalResult()
        {
            return fullScore / problemCount;
        }
        public void addCurrentResult()
        {
            fullScore += currentScore;
            currentScore = 0;
        }

        private string compare()
        {
            string result = "";
            int score = 0;
            int index = 0;
            index = FindEcho();
            if (currentTask.basicEcho.TokenAttributeValue == "")
                score += 10;
            else
            if (index == -1)
            {
                result += "Ответ не выведен.\n";
            }
            else
            {
                input.takeOp(input.ThreeAddressCode[index].FirstOperand);
                if (input.currentInt.value == Convert.ToInt32(currentTask.basicEcho.TokenAttributeValue))
                {
                    result += "Выведен верный ответ.\n";
                    score += 10;
                }
                if (input.currentInt.name == currentTask.basicEcho.TokenAttributeValue)
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

            index = FindInput();
            if (currentTask.basicInput.TokenAttributeValue == "")
            {
                score += 10;
            }
            else
            if (index == -1)
            {
                result += "В условии задано ввести переменную " + currentTask.basicInput.TokenAttributeValue + " с клавиатуры.\n";
            }
            else
            {
                input.takeOp(input.ThreeAddressCode[index].FirstOperand);
                if (input.currentInt.name == currentTask.basicInput.TokenAttributeValue)
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
                if (input.source.Identifiers.intTable.ContainsKey(currentTask.basicID.TokenAttributeValue))
                {
                    score += 15;
                    if (currentTask.basicID.TokenAttributeValue == "")
                        score += 15;
                    else
                    if (input.source.Identifiers.intTable[currentTask.basicID.TokenAttributeValue].value == Convert.ToInt32(currentTask.basicID.TokenAttributeValue))
                        score += 15;
                }
            }
            else
            {
                result += "Заданная условием переменная " + currentTask.basicID.TokenAttributeValue + " не найдена в программе.\n";
            }
            result += "Программа выполнена на " + score.ToString() + "% в соответствии с условием.";
            if (currentScore < score)
                currentScore = score;
            return result;
        }
        public void InitializeBasic()
        {
            ClearTask();
            currentTask.basicEcho.TokenAttributeValue = "50";
            currentTask.basicEcho.TokenAttributeValue = "A";
            currentTask.triadCount = 2;
            currentTask.basicID.TokenAttributeValue = "A";
            currentTask.basicID.Token = TranslationToken.Identifier;
            currentTask.basicID.TokenAttributeValue = "50";
            currentTask.basicInput.TokenAttributeValue = "";
            tasks.Enqueue(currentTask);
            currentProblem.problemImage = true;
            currentProblem.problemIndex = 1;
            currentProblem.problemText = "Выполните действия, заданные алгоритмом на рисунке 1.\nДля вывода переменной используйте оператор 'echo <выражение>;' ";
            currentProblem.problem_annotation = "Рисунок 1. Вывод переменной A.";
            problemsToOut.Enqueue(currentProblem);

            ClearTask();
            currentTask.basicEcho.TokenAttributeValue = "1";
            currentTask.basicEcho.TokenAttributeValue = "B";
            currentTask.triadCount = 5;
            currentTask.basicID.TokenAttributeValue = "A";
            currentTask.basicID.Token = TranslationToken.Identifier;

            currentTask.basicInput.TokenAttributeValue = "A";
            tasks.Enqueue(currentTask);
            currentProblem.problemImage = true;
            currentProblem.problemIndex = 2;
            currentProblem.problemText = "Выполните действия, заданные алгоритмом на рисунке 2. Переменную А введите с клавиатуры. Значение введенной переменной должно быть равно 6.";
            currentProblem.problem_annotation = "Рисунок 2. Вывод переменной B.";
            problemsToOut.Enqueue(currentProblem);
            ClearTask();

            currentTask.basicEcho.TokenAttributeValue = "120";
            currentTask.basicEcho.TokenAttributeValue = "RESULT";
            currentTask.triadCount = 18;
            currentTask.basicID.TokenAttributeValue = "COUNTER";
            currentTask.basicID.Token = TranslationToken.Identifier;

            currentTask.basicInput.TokenAttributeValue = "FACT";
            tasks.Enqueue(currentTask);
            currentProblem.problemImage = true;
            currentProblem.problemIndex = 3;
            currentProblem.problemText = "Напишите программу, вычисляющую факториал числа 5 (5!). Определите, необходимые для работы алгоритма, переменные самостоятельно. Алгоритм описан на рисунке 3.\nПеременную fact введите с клавиатуры.";
            currentProblem.problem_annotation = "Рисунок 3. Вывод переменной B.";
            problemsToOut.Enqueue(currentProblem);
            ClearTask();

        }

        private void ClearTask()
        {
            Buffer.Clear();
            currentTask.basicEcho = Buffer;
            currentTask.basicID = Buffer;
            currentTask.basicInput = Buffer;

            TriadaBuffer.Clear();
            currentTask.basicLoop = TriadaBuffer;

            currentProblem.problemImage = false;
            currentProblem.problemText = "";
            currentProblem.problemIndex = 0;
            currentProblem.problem_annotation = "";
        }

        private int FindInput()
        {
            for (int i = 0; i < input.ThreeAddressCode.Count(); ++i)
                if (input.ThreeAddressCode[i].Operation.Token == TranslationToken.InputKeyword)
                    return i;
            return -1;
        }

        private int FindEcho()
        {
            for (int i = input.ThreeAddressCode.Count() - 1; i > 0; --i)
                if (input.ThreeAddressCode[i].Operation.Token == TranslationToken.EchoKeyword)
                    return i;
            return -1;
        }

        public bool GetNextProblem()
        {
            if (tasks.Count > 0 && problemsToOut.Count > 0)
            {
                currentTask = tasks.Dequeue();
                currentProblem = problemsToOut.Dequeue();
                return true;
            }
            return false;
        }
    }

}