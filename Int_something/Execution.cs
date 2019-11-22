using Interpreter.TranslationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interpreter
{
    
    class Execution : Triad
    {
        IOform Ibuf;
        public string error;
        private int pointer = 0;
        private bool outFlag = false;
        public string [] triadResult;
        public id_Table.ID_int currentInt;
        id_Table.ID_bool currentBool;
        id_Table.ID_int bufInt;
        id_Table.ID_bool bufBool;
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
                if (ThreeAddressCode[pointer].Operation.AttributeValue == "OPERATION" || ThreeAddressCode[pointer].Operation.AttributeValue == "COMPARSION")
                    ProcessOperation();
                actionCase();
            }

            Ibuf.Dispose();
        }
        private void actionCase()
        {
            switch (this.ThreeAddressCode[pointer].Operation.Token)
            {
                case '=':
                    AssignAction();
                    ++pointer;
                    break;
                case 'e':
                    if (takeOp(ThreeAddressCode[pointer].FirstOperand))
                        MessageBox.Show(currentInt.value.ToString(), "Сообщение             ");
                    else 
                        MessageBox.Show(currentBool.value.ToString(), "Сообщение            ");
                    ++pointer;
                    break;
                case 'i':
                    if (ThreeAddressCode[pointer].FirstOperand.Token != 'X')
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
                    catch(Exception e)
                    {
                        error = "Строка [" + (ThreeAddressCode[pointer].Operation.StringNumber + 1).ToString() + "]. Неверные данные .\n" + e.Message;
                        outFlag = true;
                        return;
                    }
                    currentInt.name = ThreeAddressCode[pointer].FirstOperand.Value;
                    currentInt.numberInProgram = ThreeAddressCode[pointer].FirstOperand.numberInProgram;
                    currentInt.value = Convert.ToInt32(Ibuf.textBox1.Text);
                    source.Identifiers.intTable[currentInt.name] = currentInt;
                    pointer++;
                    break;
                case '>':
                    if (ThreeAddressCode[pointer].Operation.isConditionalBranch)
                        if (triadResult[pointer - 1] == "FALSE")
                            if (findLabel(ThreeAddressCode[pointer].Operation.StringNumber) != -1)
                            {
                                triadResult[pointer] = ThreeAddressCode[pointer].Operation.Value;
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
                        if (findLabel(ThreeAddressCode[pointer].Operation.numberInProgram) != -1)
                        {
                            triadResult[pointer] = ThreeAddressCode[pointer].Operation.Value;
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
        int findLabel(int label)
        {
            for (int i = 0; i < ThreeAddressCode.Count(); ++i)
                if (ThreeAddressCode[i].Operation.Token == ':' && ThreeAddressCode[i].Operation.StringNumber == label)
                        return i;
            return -1;
        }
        bool checkID(LexicalToken input)
        {
            if (source.Identifiers.isIdentifierExists(input))
                return true;
            return false;
        }
        void operationCase(bool isInt)
        {
            if (isInt)
                try {
                    switch (ThreeAddressCode[pointer].Operation.Token)
                    {
                        case '+':
                            triadResult[pointer] = (bufInt.value + currentInt.value).ToString();
                            break;
                        case '-':
                            triadResult[pointer] = (bufInt.value - currentInt.value).ToString();
                            break;
                        case '*':
                            triadResult[pointer] = (bufInt.value * currentInt.value).ToString();
                            break;
                        case 'c':
                            ProcessComparsion(ThreeAddressCode[pointer].Operation, isInt);
                            break;
                        case '/':
                            try
                            {
                                triadResult[pointer] = ((int)(bufInt.value / currentInt.value)).ToString();
                            }
                            catch (DivideByZeroException)
                            {
                                error = "Строка [" + (ThreeAddressCode[pointer].Operation.StringNumber +1).ToString() + "]. Деление на 0.";
                                outFlag = true;
                                return;
                            }
                            break;
                        case '%':
                            try
                            {
                                triadResult[pointer] = ((int)(bufInt.value % currentInt.value)).ToString();
                            }
                            catch (DivideByZeroException)
                            {
                                error = "Строка [" + (ThreeAddressCode[pointer].Operation.StringNumber +1).ToString() + "]. Деление на 0.";
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
                if (ThreeAddressCode[pointer].Operation.Token != 'c')
                {
                    outFlag = true;
                    error = "Строка [" + (ThreeAddressCode[pointer].Operation.StringNumber + 1).ToString() + "]. Недопустимая операция для типа данных BOOL.";
                    return;
                }
                ProcessComparsion(ThreeAddressCode[pointer].Operation, isInt);

            }
        } 
        void ProcessComparsion(LexicalToken input, bool isInt)
        {
            if (!isInt)
                switch (input.Value)
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
                switch (input.Value)
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
        void ProcessOperation()
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
            if (s!=f)
            {
                outFlag = true;
                error = "Строка [" + (ThreeAddressCode[pointer].FirstOperand.StringNumber + 1).ToString() + "]. Несовместимость типов данных : '" 
                    + ThreeAddressCode[pointer].FirstOperand.Value + "' и '" + ThreeAddressCode[pointer].SecondOperand.Value + "'";
                return;
            }
            operationCase(s);

        }
        void clearCurrent()
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
                case 'X':
                    if (checkID(input))
                    {
                        if (source.Identifiers.intTable.ContainsKey(input.Value))
                        {
                            GetCurrentIntByID(input);
                            return  true;
                        }
                        else
                            if (source.Identifiers.boolTable.ContainsKey(input.Value))
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
                case 'C':
                    return GetCurrentConst(input);
                case 'T':
                    {
                        if (CompareBool(triadResult[input.numberInProgram], true))
                        {
                            currentBool.value = true;
                            currentBool.numberInProgram = input.numberInProgram;
                            return false;
                        }
                        if (CompareBool(triadResult[input.numberInProgram], false))
                        {
                            currentBool.value = false;
                            currentBool.numberInProgram = input.numberInProgram;
                            return false;
                        }
                        currentInt.value = Convert.ToInt32(triadResult[input.numberInProgram]);
                        return true;
                    }
            }
            return false;
        }

        bool GetCurrentConst(LexicalToken input)
        {
            if (CompareBool(input.Value, true))
            {
                currentBool.value = true;
                currentBool.numberInProgram = input.numberInProgram;
                return false;
            }
            if (CompareBool(input.Value, false))
            {
                currentBool.value = false;
                currentBool.numberInProgram = input.numberInProgram;
                return false;
            }
            currentInt.value = Convert.ToInt32(input.Value);
            currentBool.numberInProgram = input.numberInProgram;
            return true;
        }
        void GetCurrentIntByID(LexicalToken input)
        {
            currentInt.name = input.Value;
            currentInt.value = source.Identifiers.intTable[currentInt.name].value;
            currentInt.numberInProgram = input.numberInProgram;
        }
        void GetCurrentBoolByID(LexicalToken input)
        {
            currentBool.name = input.Value;
            currentBool.value = source.Identifiers.boolTable[currentBool.name].value;
            currentBool.numberInProgram = input.numberInProgram;
        }
        void ClearBuff()
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
                if (source.Identifiers.intTable.ContainsKey(ThreeAddressCode[pointer].FirstOperand.Value))
                {
                    switch (ThreeAddressCode[pointer].SecondOperand.Token)
                    {
                        case 'C':
                            if (ThreeAddressCode[pointer].SecondOperand.Value == "TRUE" || ThreeAddressCode[pointer].SecondOperand.Value == "FALSE")
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(ThreeAddressCode[pointer].FirstOperand.StringNumber + 1) +
                                    "] невозможно присвоить переменной типа 'INT' значение переменной '" + ThreeAddressCode[pointer].SecondOperand.Value + "'";
                                return;
                            }
                            currentInt.name = ThreeAddressCode[pointer].FirstOperand.Value;
                            currentInt.value = Convert.ToInt32(ThreeAddressCode[pointer].SecondOperand.Value);
                            currentInt.numberInProgram = ThreeAddressCode[pointer].FirstOperand.numberInProgram;
                            source.Identifiers.intTable[currentInt.name] = currentInt;
                            ClearBuff();
                            break;

                        case 'X':
                            if (source.Identifiers.boolTable.ContainsKey(ThreeAddressCode[pointer].SecondOperand.Value))
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(ThreeAddressCode[pointer].FirstOperand.StringNumber + 1) +
                                        "] невозможно присвоить переменной типа 'INT' значение переменной '" + ThreeAddressCode[pointer].SecondOperand.Value + "'";
                                return;
                            }
                            currentInt.name = ThreeAddressCode[pointer].FirstOperand.Value;
                            currentInt.value = source.Identifiers.intTable[ThreeAddressCode[pointer].SecondOperand.Value].value;
                            currentInt.numberInProgram = ThreeAddressCode[pointer].FirstOperand.numberInProgram;
                            source.Identifiers.intTable[currentInt.name] = currentInt;
                            ClearBuff();
                            break;
                        case 'T':
                            if (triadResult[ThreeAddressCode[pointer].SecondOperand.numberInProgram] != "" &&
                                triadResult[ThreeAddressCode[pointer].SecondOperand.numberInProgram] != "TRUE" &&
                                triadResult[ThreeAddressCode[pointer].SecondOperand.numberInProgram] != "FALSE")
                            {
                                currentInt.name = ThreeAddressCode[pointer].FirstOperand.Value;
                                currentInt.value = Convert.ToInt32(triadResult[ThreeAddressCode[pointer].SecondOperand.numberInProgram]);
                                currentInt.numberInProgram = ThreeAddressCode[pointer].FirstOperand.numberInProgram;
                                source.Identifiers.intTable[currentInt.name] = currentInt;
                                ClearBuff();
                            }
                            else
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(ThreeAddressCode[pointer].FirstOperand.StringNumber + 1).ToString() +
                                    "] невозможно присвоить переменной типа 'INT' значение '" + ThreeAddressCode[pointer].SecondOperand.Value + "'";
                                return;
                            }
                            break;
                    }
                    return;
                }
                if (source.Identifiers.boolTable.ContainsKey(ThreeAddressCode[pointer].FirstOperand.Value))
                {
                    switch (ThreeAddressCode[pointer].SecondOperand.Token)
                    {
                        case 'C':
                            if (ThreeAddressCode[pointer].SecondOperand.Value != "TRUE" && ThreeAddressCode[pointer].SecondOperand.Value != "FALSE")
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(ThreeAddressCode[pointer].FirstOperand.StringNumber + 1).ToString() +
                                    "] невозможно присвоить переменной типа 'BOOL' значение '" + ThreeAddressCode[pointer].SecondOperand.Value + "'";
                                return;
                            }
                            currentBool.name = ThreeAddressCode[pointer].FirstOperand.Value;
                            currentBool.value = Convert.ToBoolean(ThreeAddressCode[pointer].SecondOperand.Value);
                            currentBool.numberInProgram = ThreeAddressCode[pointer].FirstOperand.numberInProgram;
                            source.Identifiers.boolTable[currentBool.name] = currentBool;
                            ClearBuff();
                            break;
                        case 'X':
                            if (source.Identifiers.intTable.ContainsKey(ThreeAddressCode[pointer].SecondOperand.Value))
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(ThreeAddressCode[pointer].FirstOperand.StringNumber + 1).ToString() +
                                        "] невозможно присвоить переменной типа 'INT' значение '" + ThreeAddressCode[pointer].SecondOperand.Value + "'";
                                return;
                            }
                            currentBool.name = ThreeAddressCode[pointer].FirstOperand.Value;
                            currentBool.value = source.Identifiers.boolTable[ThreeAddressCode[pointer].SecondOperand.Value].value;
                            currentBool.numberInProgram = ThreeAddressCode[pointer].FirstOperand.numberInProgram;
                            source.Identifiers.boolTable[currentBool.name] = currentBool;
                            ClearBuff();
                            break;
                        case 'T':
                            if (triadResult[ThreeAddressCode[pointer].SecondOperand.numberInProgram] == "TRUE" ||
                                triadResult[ThreeAddressCode[pointer].SecondOperand.numberInProgram] == "FALSE")
                            {
                                currentBool.name = ThreeAddressCode[pointer].FirstOperand.Value;
                                currentBool.value = Convert.ToBoolean(triadResult[ThreeAddressCode[pointer].SecondOperand.numberInProgram]);
                                currentBool.numberInProgram = ThreeAddressCode[pointer].FirstOperand.numberInProgram;
                                source.Identifiers.boolTable[currentBool.name] = currentBool;
                                ClearBuff();
                            }
                            else
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(ThreeAddressCode[pointer].FirstOperand.StringNumber + 1).ToString() +
                                    "] невозможно присвоить переменной типа 'BOOL' значение '" + triadResult[ThreeAddressCode[pointer].SecondOperand.numberInProgram] + "'";
                                return;
                            }
                            break;
                    }
                }
            }
            else
            {
                error = "Идентификатор " + ThreeAddressCode[pointer].FirstOperand.Value + " не обьявлен. Невозможно присвоить значение.";
                outFlag = true;
                return;
            }
        }

        private bool CompareBool(string value, bool boolValue) =>
            string.Equals(value, boolValue.ToString(), StringComparison.InvariantCultureIgnoreCase);
        
    }
}