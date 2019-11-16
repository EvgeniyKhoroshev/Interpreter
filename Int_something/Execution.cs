using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Int_something
{
    
    class Execution : Triad
    {
        IDisposable dispose;
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

            triadResult = new string[T_AC.Count()];
            while (pointer < this.T_AC.Count() && !outFlag) 
            {
                if (T_AC[pointer].operation.AttributeValue == "OPERATION" || T_AC[pointer].operation.AttributeValue == "COMPARSION")
                    operationAction();
                actionCase();
            }
            dispose = Ibuf;
            Ibuf.Dispose();
        }
        private void actionCase()
        {
            switch (this.T_AC[pointer].operation.Token)
            {
                case '=':
                    assignAction();
                    ++pointer;
                    break;
                case 'e':
                    if (takeOp(T_AC[pointer].operator_1))
                        MessageBox.Show(currentInt.value.ToString(), "Сообщение             ");
                    else 
                        MessageBox.Show(currentBool.value.ToString(), "Сообщение            ");
                    ++pointer;
                    break;
                case 'i':
                    if (T_AC[pointer].operator_1.Token != 'X')
                    {
                        outFlag = true;
                        error = "Строка [" + (T_AC[pointer].operation.StringNumber + 1).ToString() + "]. Оператор ввода должен содержать целочисленную переменную 'input X;' .";
                        return;
                    }
                    Ibuf.ShowDialog();
                    try
                    {
                        Convert.ToInt32(Ibuf.textBox1.Text);
                    }
                    catch(Exception e)
                    {
                        error = "Строка [" + (T_AC[pointer].operation.StringNumber + 1).ToString() + "]. Неверные данные .\n" + e.Message;
                        outFlag = true;
                        return;
                    }
                    currentInt.name = T_AC[pointer].operator_1.Value;
                    currentInt.numberInProgram = T_AC[pointer].operator_1.numberInProgram;
                    currentInt.value = Convert.ToInt32(Ibuf.textBox1.Text);
                    source.Identifiers.intTable[currentInt.name] = currentInt;
                    pointer++;
                    break;
                case '>':
                    if (T_AC[pointer].operation.isConditionalBranch)
                        if (triadResult[pointer - 1] == "FALSE")
                            if (findLabel(T_AC[pointer].operation.StringNumber) != -1)
                            {
                                triadResult[pointer] = T_AC[pointer].operation.Value;
                                pointer = findLabel(T_AC[pointer].operation.StringNumber);
                                break;
                            }
                            else
                            {
                                error = "Строка [" + (T_AC[pointer].operation.StringNumber + 1).ToString() + "]. Метка не найдена.";
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
                        if (findLabel(T_AC[pointer].operation.numberInProgram) != -1)
                        {
                            triadResult[pointer] = T_AC[pointer].operation.Value;
                            pointer = findLabel(T_AC[pointer].operation.StringNumber);
                            break;
                        }
                        else
                        {
                            error = "Строка [" + (T_AC[pointer].operation.StringNumber + 1).ToString() + "]. Метка не найдена.";
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
            for (int i = 0; i < T_AC.Count(); ++i)
                if (T_AC[i].operation.Token == ':' && T_AC[i].operation.StringNumber == label)
                        return i;
            return -1;
        }
        bool checkID(translationTable input)
        {
            if (source.Identifiers.isIdentifierExists(input))
                return true;
            return false;
        }
        void operationCase(bool isInt)
        {
            if (isInt)
                try {
                    switch (T_AC[pointer].operation.Token)
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
                            caseOfcompares(T_AC[pointer].operation, isInt);
                            break;
                        case '/':
                            try
                            {
                                triadResult[pointer] = ((int)(bufInt.value / currentInt.value)).ToString();
                            }
                            catch (DivideByZeroException)
                            {
                                error = "Строка [" + (T_AC[pointer].operation.StringNumber +1).ToString() + "]. Деление на 0.";
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
                                error = "Строка [" + (T_AC[pointer].operation.StringNumber +1).ToString() + "]. Деление на 0.";
                                outFlag = true;
                                return;
                            }
                            break;
                    }
                }
                catch (OverflowException)
                {
                    error = "Строка [" + (T_AC[pointer].operation.StringNumber + 1).ToString() + "]. Переполнение разрядной сетки.";
                    outFlag = true;
                    return;
                }
            else
            {
                if (T_AC[pointer].operation.Token != 'c')
                {
                    outFlag = true;
                    error = "Строка [" + (T_AC[pointer].operation.StringNumber + 1).ToString() + "]. Недопустимая операция для типа данных BOOL.";
                    return;
                }
                caseOfcompares(T_AC[pointer].operation, isInt);

            }
        } 
        void caseOfcompares(translationTable input, bool isInt)
        {
            if (!isInt)
                switch (input.Value)
                {
                    case "==":
                        if (currentBool.value == bufBool.value)
                            triadResult[pointer] = "TRUE";
                        else
                            triadResult[pointer] = "FALSE";
                        break;
                    case "<>":
                        if (currentBool.value != bufBool.value)
                            triadResult[pointer] = "TRUE";
                        else
                            triadResult[pointer] = "FALSE";
                        break;
                    case ">":
                    case "<":
                        error = "Строка [" + (T_AC[pointer].operation.StringNumber + 1).ToString() + "]. Недопустимая операция для типа данных BOOL.";
                        break;
                }
            else
                switch (input.Value)
                {
                    case "==":
                        if (bufInt.value == currentInt.value)
                            triadResult[pointer] = "TRUE";
                        else
                            triadResult[pointer] = "FALSE";
                        break;
                    case "<>":
                        if (bufInt.value != currentInt.value)
                            triadResult[pointer] = "TRUE";
                        else
                            triadResult[pointer] = "FALSE";
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
        void operationAction()
        {
            clearBuff();
            bool f = false, s = false;
            f = takeOp(T_AC[pointer].operator_1);
            if (f)
                bufInt = currentInt;
            else
                bufBool = currentBool;
            clearCurrent();
            s = takeOp(T_AC[pointer].operator_2);
            if (s!=f)
            {
                outFlag = true;
                error = "Строка [" + (T_AC[pointer].operator_1.StringNumber + 1).ToString() + "]. Несовместимость типов данных : '" 
                    + T_AC[pointer].operator_1.Value + "' и '" + T_AC[pointer].operator_2.Value + "'";
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
        public bool takeOp(translationTable input)
        {
            switch (input.Token)
            {
                case 'X':
                    if (checkID(input))
                    {
                        if (source.Identifiers.intTable.ContainsKey(input.Value))
                        {
                            getCurrentIntByID(input);
                            return  true;
                        }
                        else
                            if (source.Identifiers.boolTable.ContainsKey(input.Value))
                        {
                            getCurrentBoolByID(input);
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
                    return getCurrentConst(input);
                case 'T':
                    {
                        if (triadResult[input.numberInProgram] == "TRUE")
                        {
                            currentBool.value = true;
                            currentBool.numberInProgram = input.numberInProgram;
                            return false;
                        }
                        if (triadResult[input.numberInProgram] == "FALSE")
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

        bool getCurrentConst(translationTable input)
        {
            if (input.Value == "TRUE")
            {
                currentBool.value = true;
                currentBool.numberInProgram = input.numberInProgram;
                return false;
            }
            if (input.Value == "FALSE")
            {
                currentBool.value = false;
                currentBool.numberInProgram = input.numberInProgram;
                return false;
            }
            currentInt.value = Convert.ToInt32(input.Value);
            currentBool.numberInProgram = input.numberInProgram;
            return true;
        }
        void getCurrentIntByID(translationTable input)
        {
            currentInt.name = input.Value;
            currentInt.value = source.Identifiers.intTable[currentInt.name].value;
            currentInt.numberInProgram = input.numberInProgram;
        }
        void getCurrentBoolByID(translationTable input)
        {
            currentBool.name = input.Value;
            currentBool.value = source.Identifiers.boolTable[currentBool.name].value;
            currentBool.numberInProgram = input.numberInProgram;
        }
        void clearBuff()
        {
            bufInt.name = "";
            bufInt.numberInProgram = 0;
            bufInt.value = 0;
            bufBool.name = "";
            bufBool.numberInProgram = 0;
            bufBool.value = false;
        }
        private void assignAction()
        {
            if (checkID(T_AC[pointer].operator_1))
            {
                if (source.Identifiers.intTable.ContainsKey(T_AC[pointer].operator_1.Value))
                {
                    switch (T_AC[pointer].operator_2.Token)
                    {
                        case 'C':
                            if (T_AC[pointer].operator_2.Value == "TRUE" || T_AC[pointer].operator_2.Value == "FALSE")
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(T_AC[pointer].operator_1.StringNumber + 1) +
                                    "] невозможно присвоить переменной типа 'INT' значение переменной '" + T_AC[pointer].operator_2.Value + "'";
                                return;
                            }
                            currentInt.name = T_AC[pointer].operator_1.Value;
                            currentInt.value = Convert.ToInt32(T_AC[pointer].operator_2.Value);
                            currentInt.numberInProgram = T_AC[pointer].operator_1.numberInProgram;
                            source.Identifiers.intTable[currentInt.name] = currentInt;
                            clearBuff();
                            break;

                        case 'X':
                            if (source.Identifiers.boolTable.ContainsKey(T_AC[pointer].operator_2.Value))
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(T_AC[pointer].operator_1.StringNumber + 1) +
                                        "] невозможно присвоить переменной типа 'INT' значение переменной '" + T_AC[pointer].operator_2.Value + "'";
                                return;
                            }
                            currentInt.name = T_AC[pointer].operator_1.Value;
                            currentInt.value = source.Identifiers.intTable[T_AC[pointer].operator_2.Value].value;
                            currentInt.numberInProgram = T_AC[pointer].operator_1.numberInProgram;
                            source.Identifiers.intTable[currentInt.name] = currentInt;
                            clearBuff();
                            break;
                        case 'T':
                            if (triadResult[T_AC[pointer].operator_2.numberInProgram] != "" &&
                                triadResult[T_AC[pointer].operator_2.numberInProgram] != "TRUE" &&
                                triadResult[T_AC[pointer].operator_2.numberInProgram] != "FALSE")
                            {
                                currentInt.name = T_AC[pointer].operator_1.Value;
                                currentInt.value = Convert.ToInt32(triadResult[T_AC[pointer].operator_2.numberInProgram]);
                                currentInt.numberInProgram = T_AC[pointer].operator_1.numberInProgram;
                                source.Identifiers.intTable[currentInt.name] = currentInt;
                                clearBuff();
                            }
                            else
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(T_AC[pointer].operator_1.StringNumber + 1).ToString() +
                                    "] невозможно присвоить переменной типа 'INT' значение '" + T_AC[pointer].operator_2.Value + "'";
                                return;
                            }
                            break;
                    }
                    return;
                }
                if (source.Identifiers.boolTable.ContainsKey(T_AC[pointer].operator_1.Value))
                {
                    switch (T_AC[pointer].operator_2.Token)
                    {
                        case 'C':
                            if (T_AC[pointer].operator_2.Value != "TRUE" && T_AC[pointer].operator_2.Value != "FALSE")
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(T_AC[pointer].operator_1.StringNumber + 1).ToString() +
                                    "] невозможно присвоить переменной типа 'BOOL' значение '" + T_AC[pointer].operator_2.Value + "'";
                                return;
                            }
                            currentBool.name = T_AC[pointer].operator_1.Value;
                            currentBool.value = Convert.ToBoolean(T_AC[pointer].operator_2.Value);
                            currentBool.numberInProgram = T_AC[pointer].operator_1.numberInProgram;
                            source.Identifiers.boolTable[currentBool.name] = currentBool;
                            clearBuff();
                            break;
                        case 'X':
                            if (source.Identifiers.intTable.ContainsKey(T_AC[pointer].operator_2.Value))
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(T_AC[pointer].operator_1.StringNumber + 1).ToString() +
                                        "] невозможно присвоить переменной типа 'INT' значение '" + T_AC[pointer].operator_2.Value + "'";
                                return;
                            }
                            currentBool.name = T_AC[pointer].operator_1.Value;
                            currentBool.value = source.Identifiers.boolTable[T_AC[pointer].operator_2.Value].value;
                            currentBool.numberInProgram = T_AC[pointer].operator_1.numberInProgram;
                            source.Identifiers.boolTable[currentBool.name] = currentBool;
                            clearBuff();
                            break;
                        case 'T':
                            if (triadResult[T_AC[pointer].operator_2.numberInProgram] == "TRUE" ||
                                triadResult[T_AC[pointer].operator_2.numberInProgram] == "FALSE")
                            {
                                currentBool.name = T_AC[pointer].operator_1.Value;
                                currentBool.value = Convert.ToBoolean(triadResult[T_AC[pointer].operator_2.numberInProgram]);
                                currentBool.numberInProgram = T_AC[pointer].operator_1.numberInProgram;
                                source.Identifiers.boolTable[currentBool.name] = currentBool;
                                clearBuff();
                            }
                            else
                            {
                                outFlag = true;
                                error = "Строка [" + Convert.ToString(T_AC[pointer].operator_1.StringNumber + 1).ToString() +
                                    "] невозможно присвоить переменной типа 'BOOL' значение '" + triadResult[T_AC[pointer].operator_2.numberInProgram] + "'";
                                return;
                            }
                            break;
                    }
                }
            }
            else
            {
                error = "Идентификатор " + T_AC[pointer].operator_1.Value + " не обьявлен. Невозможно присвоить значение.";
                outFlag = true;
                return;
            }
        }
    }
}