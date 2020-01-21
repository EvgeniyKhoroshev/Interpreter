﻿using Interpreter.Translation;
using Interpreter.TranslationResult;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Interpreter
{
    internal class Execution : Triad
    {
        private IOform Ibuf;
        public string error;
        private int pointer = 0;
        private bool outFlag = false;
        public string[] triadResult;
        public id_Table.ID_int currentInt;
        private id_Table.ID_bool currentBool;
        private id_Table.ID_int bufInt;
        private id_Table.ID_bool bufBool;
        public Execution()
        {
            Ibuf = new IOform();
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

        public void exec()
        {

            triadResult = new string[ThreeAddressCode.Count()];
            while (pointer < this.ThreeAddressCode.Count() && !outFlag)
            {
                if (ThreeAddressCode[pointer].Operation.TokenAttributeValue == "OPERATION" || ThreeAddressCode[pointer].Operation.TokenAttributeValue == "COMPARSION")
                    ProcessOperation();
                actionCase();
            }

            Ibuf.Dispose();
        }
        private void actionCase()
        {
            switch (this.ThreeAddressCode[pointer].Operation.Token)
            {
                case TranslationToken.AssignOperation:
                    AssignAction();
                    ++pointer;
                    break;
                case TranslationToken.EchoKeyword:
                    if (takeOp(ThreeAddressCode[pointer].FirstOperand))
                        MessageBox.Show(currentInt.value.ToString(), "Сообщение             ");
                    else
                        MessageBox.Show(currentBool.value.ToString(), "Сообщение            ");
                    ++pointer;
                    break;
                case TranslationToken.InputKeyword:
                    if (ThreeAddressCode[pointer].FirstOperand.Token != TranslationToken.Identifier)
                    {
                        outFlag = true;
                        error = "Строка [" + (ThreeAddressCode[pointer].Operation.StringNumber + 1).ToString() + "]. Оператор ввода должен содержать целочисленную переменную 'input X;' .";
                        return;
                    }
                    Ibuf.ShowDialog();
                    try
                    {
                        Convert.ToInt32(Ibuf.textBox1.Text);
                    }
                    catch (Exception e)
                    {
                        error = "Строка [" + (ThreeAddressCode[pointer].Operation.StringNumber + 1).ToString() + "]. Неверные данные .\n" + e.Message;
                        outFlag = true;
                        return;
                    }
                    currentInt.name = ThreeAddressCode[pointer].FirstOperand.TokenAttributeValue;
                    currentInt.numberInProgram = ThreeAddressCode[pointer].FirstOperand.LexemeStartIndex;
                    currentInt.value = Convert.ToInt32(Ibuf.textBox1.Text);
                    source.Identifiers.intTable[currentInt.name] = currentInt;
                    pointer++;
                    break;
                case TranslationToken.GotoTransition:
                    if (ThreeAddressCode[pointer].Operation.isConditionalBranch)
                        if (triadResult[pointer - 1] == "FALSE")
                            if (findLabel(ThreeAddressCode[pointer].Operation.StringNumber) != -1)
                            {
                                triadResult[pointer] = ThreeAddressCode[pointer].Operation.TokenAttributeValue;
                                pointer = findLabel(ThreeAddressCode[pointer].Operation.StringNumber);
                                break;
                            }
                            else
                            {
                                error = "Строка [" + (ThreeAddressCode[pointer].Operation.StringNumber + 1).ToString() + "]. Метка не найдена.";
                                outFlag = true;
                                return;
                            }
                        else
                        {
                            pointer++;
                            break;
                        }
                    else
                    {
                        if (findLabel(ThreeAddressCode[pointer].Operation.LexemeStartIndex) != -1)
                        {
                            triadResult[pointer] = ThreeAddressCode[pointer].Operation.TokenAttributeValue;
                            pointer = findLabel(ThreeAddressCode[pointer].Operation.StringNumber);
                            break;
                        }
                        else
                        {
                            error = "Строка [" + (ThreeAddressCode[pointer].Operation.StringNumber + 1).ToString() + "]. Метка не найдена.";
                            outFlag = true;
                            return;
                        }
                    }
                default:
                    ++pointer;
                    break;
            }
        }

        private int findLabel(int label)
        {
            for (int i = 0; i < ThreeAddressCode.Count(); ++i)
                if (ThreeAddressCode[i].Operation.Token == TranslationToken.GotoLabel && ThreeAddressCode[i].Operation.StringNumber == label)
                    return i;
            return -1;
        }

        private bool checkID(LexicalToken input)
        {
            if (source.Identifiers.isIdentifierExists(input))
                return true;
            return false;
        }

        private void operationCase(bool isInt)
        {
            if (isInt)
                try
                {
                    switch (ThreeAddressCode[pointer].Operation.Token)
                    {
                        case TranslationToken.PlusOperation:
                            triadResult[pointer] = (bufInt.value + currentInt.value).ToString();
                            break;
                        case TranslationToken.MinusOperation:
                            triadResult[pointer] = (bufInt.value - currentInt.value).ToString();
                            break;
                        case TranslationToken.MultipleOperation:

                            triadResult[pointer] = (bufInt.value * currentInt.value).ToString();
                            break;
                        case TranslationToken.Constant:
                            ProcessComparsion(ThreeAddressCode[pointer].Operation, isInt);
                            break;
                        case TranslationToken.DivisionOperation:
                            try
                            {
                                triadResult[pointer] = ((int)(bufInt.value / currentInt.value)).ToString();
                            }
                            catch (DivideByZeroException)
                            {
                                error = "Строка [" + (ThreeAddressCode[pointer].Operation.StringNumber + 1).ToString() + "]. Деление на 0.";
                                outFlag = true;
                                return;
                            }
                            break;
                        case TranslationToken.RemainderOfTheDivisionOperation:
                            try
                            {
                                triadResult[pointer] = ((int)(bufInt.value % currentInt.value)).ToString();
                            }
                            catch (DivideByZeroException)
                            {
                                error = "Строка [" + (ThreeAddressCode[pointer].Operation.StringNumber + 1).ToString() + "]. Деление на 0.";
                                outFlag = true;
                                return;
                            }
                            break;
                    }
                }
                catch (OverflowException)
                {
                    error = "Строка [" + (ThreeAddressCode[pointer].Operation.StringNumber + 1).ToString() + "]. Переполнение разрядной сетки.";
                    outFlag = true;
                    return;
                }
            else
            {
                if (ThreeAddressCode[pointer].Operation.Token != TranslationToken.Constant)
                {
                    outFlag = true;
                    error = "Строка [" + (ThreeAddressCode[pointer].Operation.StringNumber + 1).ToString() + "]. Недопустимая операция для типа данных BOOL.";
                    return;
                }
                ProcessComparsion(ThreeAddressCode[pointer].Operation, isInt);

            }
        }

        private void ProcessComparsion(LexicalToken input, bool isInt)
        {
            if (!isInt)
                switch (input.TokenAttributeValue)
                {
                    case "==":
                        triadResult[pointer] = (currentBool.value == bufBool.value).ToString();
                        break;
                    case "<>":
                        triadResult[pointer] = (currentBool.value != bufBool.value).ToString();
                        break;
                    case ">":
                    case "<":
                        error = "Строка [" + (ThreeAddressCode[pointer].Operation.StringNumber + 1).ToString()
                            + "]. Недопустимая операция для типа данных BOOL.";
                        break;
                }
            else
                switch (input.TokenAttributeValue)
                {
                    case "==":
                        triadResult[pointer] = (bufInt.value == currentInt.value).ToString();
                        break;
                    case "<>":
                        triadResult[pointer] = (bufInt.value != currentInt.value).ToString();
                        break;
                    case ">":
                        if (bufInt.value > currentInt.value)
                            triadResult[pointer] = "TRUE";
                        else
                            triadResult[pointer] = "FALSE";
                        break;
                    case "<":
                        if (bufInt.value < currentInt.value)
                            triadResult[pointer] = "TRUE";
                        else
                            triadResult[pointer] = "FALSE";
                        break;
                }
        }

        private void ProcessOperation()
        {
            ClearBuff();
            bool f = false, s = false;
            f = takeOp(ThreeAddressCode[pointer].FirstOperand);
            if (f)
                bufInt = currentInt;
            else
                bufBool = currentBool;
            clearCurrent();
            s = takeOp(ThreeAddressCode[pointer].SecondOperand);
            if (s != f)
            {
                outFlag = true;
                error = "Строка [" + (ThreeAddressCode[pointer].FirstOperand.StringNumber + 1).ToString() + "]. Несовместимость типов данных : '"
                    + ThreeAddressCode[pointer].FirstOperand.TokenAttributeValue + "' и '" + ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue + "'";
                return;
            }
            operationCase(s);

        }

        private void clearCurrent()
        {
            currentInt.name = "";
            currentInt.numberInProgram = 0;
            currentInt.value = 0;
            currentBool.name = "";
            currentBool.numberInProgram = 0;
            currentBool.value = false;
        }
        public bool takeOp(LexicalToken input)
        {
            switch (input.Token)
            {
                case TranslationToken.Identifier:
                    if (checkID(input))
                    {
                        if (source.Identifiers.intTable.ContainsKey(input.TokenAttributeValue))
                        {
                            GetCurrentIntByID(input);
                            return true;
                        }
                        else
                            if (source.Identifiers.boolTable.ContainsKey(input.TokenAttributeValue))
                        {
                            GetCurrentBoolByID(input);
                            return false;
                        }
                        else
                        {
                            outFlag = true;
                            return false;
                        }
                    }
                    else
                    {
                        outFlag = true;
                        return false;
                    }
                case TranslationToken.Constant:
                    return GetCurrentConst(input);
                case TranslationToken.Triada:
                    {
                        if (CompareBool(triadResult[input.LexemeStartIndex], true))
                        {
                            currentBool.value = true;
                            currentBool.numberInProgram = input.LexemeStartIndex;
                            return false;
                        }
                        if (CompareBool(triadResult[input.LexemeStartIndex], false))
                        {
                            currentBool.value = false;
                            currentBool.numberInProgram = input.LexemeStartIndex;
                            return false;
                        }
                        currentInt.value = Convert.ToInt32(triadResult[input.LexemeStartIndex]);
                        return true;
                    }
            }
            return false;
        }

        private bool GetCurrentConst(LexicalToken input)
        {
            if (CompareBool(input.TokenAttributeValue, true))
            {
                currentBool.value = true;
                currentBool.numberInProgram = input.LexemeStartIndex;
                return false;
            }
            if (CompareBool(input.TokenAttributeValue, false))
            {
                currentBool.value = false;
                currentBool.numberInProgram = input.LexemeStartIndex;
                return false;
            }
            currentInt.value = Convert.ToInt32(input.TokenAttributeValue);
            currentBool.numberInProgram = input.LexemeStartIndex;
            return true;
        }

        private void GetCurrentIntByID(LexicalToken input)
        {
            currentInt.name = input.TokenAttributeValue;
            currentInt.value = source.Identifiers.intTable[currentInt.name].value;
            currentInt.numberInProgram = input.LexemeStartIndex;
        }

        private void GetCurrentBoolByID(LexicalToken input)
        {
            currentBool.name = input.TokenAttributeValue;
            currentBool.value = source.Identifiers.boolTable[currentBool.name].value;
            currentBool.numberInProgram = input.LexemeStartIndex;
        }

        private void ClearBuff()
        {
            bufInt.name = "";
            bufInt.numberInProgram = 0;
            bufInt.value = 0;
            bufBool.name = "";
            bufBool.numberInProgram = 0;
            bufBool.value = false;
        }
        private void AssignAction()
        {
            if (checkID(ThreeAddressCode[pointer].FirstOperand))
            {
                if (source.Identifiers.intTable.ContainsKey(ThreeAddressCode[pointer].FirstOperand.TokenAttributeValue))
                {
                    switch (ThreeAddressCode[pointer].SecondOperand.Token)
                    {
                        case TranslationToken.Constant: // Константа.
                            if (ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue == "TRUE" || ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue == "FALSE")
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(ThreeAddressCode[pointer].FirstOperand.StringNumber + 1) +
                                    "] невозможно присвоить переменной типа 'INT' значение переменной '" + ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue + "'";
                                return;
                            }
                            currentInt.name = ThreeAddressCode[pointer].FirstOperand.TokenAttributeValue;
                            currentInt.value = Convert.ToInt32(ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue);
                            currentInt.numberInProgram = ThreeAddressCode[pointer].FirstOperand.LexemeStartIndex;
                            source.Identifiers.intTable[currentInt.name] = currentInt;
                            ClearBuff();
                            break;

                        case TranslationToken.Identifier: // Переменная.
                            if (source.Identifiers.boolTable.ContainsKey(ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue))
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(ThreeAddressCode[pointer].FirstOperand.StringNumber + 1) +
                                        "] невозможно присвоить переменной типа 'INT' значение переменной '" + ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue + "'";
                                return;
                            }
                            currentInt.name = ThreeAddressCode[pointer].FirstOperand.TokenAttributeValue;
                            currentInt.value = source.Identifiers.intTable[ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue].value;
                            currentInt.numberInProgram = ThreeAddressCode[pointer].FirstOperand.LexemeStartIndex;
                            source.Identifiers.intTable[currentInt.name] = currentInt;
                            ClearBuff();
                            break;
                        case TranslationToken.Triada:
                            if (triadResult[ThreeAddressCode[pointer].SecondOperand.LexemeStartIndex] != "" &&
                                triadResult[ThreeAddressCode[pointer].SecondOperand.LexemeStartIndex] != "TRUE" &&
                                triadResult[ThreeAddressCode[pointer].SecondOperand.LexemeStartIndex] != "FALSE")
                            {
                                currentInt.name = ThreeAddressCode[pointer].FirstOperand.TokenAttributeValue;
                                currentInt.value = Convert.ToInt32(triadResult[ThreeAddressCode[pointer].SecondOperand.LexemeStartIndex]);
                                currentInt.numberInProgram = ThreeAddressCode[pointer].FirstOperand.LexemeStartIndex;
                                source.Identifiers.intTable[currentInt.name] = currentInt;
                                ClearBuff();
                            }
                            else
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(ThreeAddressCode[pointer].FirstOperand.StringNumber + 1).ToString() +
                                    "] невозможно присвоить переменной типа 'INT' значение '" + ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue + "'";
                                return;
                            }
                            break;
                    }
                    return;
                }
                if (source.Identifiers.boolTable.ContainsKey(ThreeAddressCode[pointer].FirstOperand.TokenAttributeValue))
                {
                    switch (ThreeAddressCode[pointer].SecondOperand.Token)
                    {
                        case TranslationToken.Constant:
                            if (ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue != "TRUE" && ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue != "FALSE")
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(ThreeAddressCode[pointer].FirstOperand.StringNumber + 1).ToString() +
                                    "] невозможно присвоить переменной типа 'BOOL' значение '" + ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue + "'";
                                return;
                            }
                            currentBool.name = ThreeAddressCode[pointer].FirstOperand.TokenAttributeValue;
                            currentBool.value = Convert.ToBoolean(ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue);
                            currentBool.numberInProgram = ThreeAddressCode[pointer].FirstOperand.LexemeStartIndex;
                            source.Identifiers.boolTable[currentBool.name] = currentBool;
                            ClearBuff();
                            break;
                        case TranslationToken.Identifier:
                            if (source.Identifiers.intTable.ContainsKey(ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue))
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(ThreeAddressCode[pointer].FirstOperand.StringNumber + 1).ToString() +
                                        "] невозможно присвоить переменной типа 'INT' значение '" + ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue + "'";
                                return;
                            }
                            currentBool.name = ThreeAddressCode[pointer].FirstOperand.TokenAttributeValue;
                            currentBool.value = source.Identifiers.boolTable[ThreeAddressCode[pointer].SecondOperand.TokenAttributeValue].value;
                            currentBool.numberInProgram = ThreeAddressCode[pointer].FirstOperand.LexemeStartIndex;
                            source.Identifiers.boolTable[currentBool.name] = currentBool;
                            ClearBuff();
                            break;
                        case TranslationToken.Triada:
                            if (triadResult[ThreeAddressCode[pointer].SecondOperand.LexemeStartIndex] == "TRUE" ||
                                triadResult[ThreeAddressCode[pointer].SecondOperand.LexemeStartIndex] == "FALSE")
                            {
                                currentBool.name = ThreeAddressCode[pointer].FirstOperand.TokenAttributeValue;
                                currentBool.value = Convert.ToBoolean(triadResult[ThreeAddressCode[pointer].SecondOperand.LexemeStartIndex]);
                                currentBool.numberInProgram = ThreeAddressCode[pointer].FirstOperand.LexemeStartIndex;
                                source.Identifiers.boolTable[currentBool.name] = currentBool;
                                ClearBuff();
                            }
                            else
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(ThreeAddressCode[pointer].FirstOperand.StringNumber + 1).ToString() +
                                    "] невозможно присвоить переменной типа 'BOOL' значение '" + triadResult[ThreeAddressCode[pointer].SecondOperand.LexemeStartIndex] + "'";
                                return;
                            }
                            break;
                    }
                }
            }
            else
            {
                error = "Идентификатор " + ThreeAddressCode[pointer].FirstOperand.TokenAttributeValue + " не обьявлен. Невозможно присвоить значение.";
                outFlag = true;
                return;
            }
        }

        private bool CompareBool(string value, bool boolValue) =>
            string.Equals(value, boolValue.ToString(), StringComparison.InvariantCultureIgnoreCase);

    }
}