using Interpreter.Constants;
using Interpreter.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace Interpreter
{
    internal class SA_LR1
    {
        public SA_LR1()
        {
            _LRTable = (int[,])FileExtensions.ReadFromFile(typeof(int[,]), PathConstants.LR1TablePath);
            _LRColumns = (string[])FileExtensions.ReadFromFile(typeof(string[]), PathConstants.LR1ColumnPath);
        }
        public struct toStack
        {
            public string sign;
            public int state;
        }

        private readonly int[,] _LRTable;
        private readonly string[] _LRColumns;

        private int loopCounter;
        private Stack<toStack> workStack, outputStack;
        private toStack buffer;
        private string inputString, outputString, stateLog;

        private int GetColByID(string ID)
        {
            if (_LRColumns.Contains(ID))
            {
                for (int i = 0; i < _LRColumns.Length; ++i)
                    if (_LRColumns[i] == ID)
                        return i;
            }

            return 0;
        }
        public string LRAnalisys(string Tokens, out string errLog, out long time, out long ticks)
        {
            workStack = new Stack<toStack>();
            outputStack = new Stack<toStack>();
            Stopwatch t = new Stopwatch();
            t.Reset();
            t.Start();
            buffer.sign = "$";
            buffer.state = 1;
            inputString = Tokens + "$";
            workStack.Push(buffer);
            buffer.sign = Convert.ToString(inputString[0]);
            try
            {
                while (workStack.Count > 0)
                {
                    if (_LRTable[buffer.state, GetColByID(buffer.sign)] < 100)
                    {
                        shift();
                    }
                    else
                        if (_LRTable[buffer.state, GetColByID(buffer.sign)] >= 100)
                    {
                        reduce();
                        continue;
                    }
                    ++loopCounter;
                    buffer.sign = Convert.ToString(inputString[loopCounter]);
                }
            }
            catch (IndexOutOfRangeException) { }
            t.Stop();
            time = t.ElapsedMilliseconds;
            ticks = t.ElapsedTicks;
            errLog = stateLog;
            return outputString;
        }

        private void shift()
        {
            stateLog += "M ( " + Convert.ToString(buffer.state) + " , " + buffer.sign + " ) = " + _LRTable[buffer.state, GetColByID(buffer.sign)] + "\n";
            buffer.state = _LRTable[buffer.state, GetColByID(buffer.sign)];
            workStack.Push(buffer);
        }

        private void reduce()
        {
            string buf = getTheRule(_LRTable[buffer.state, GetColByID(buffer.sign)]);
            stateLog += "M ( " + Convert.ToString(buffer.state) + " , " + buffer.sign + " ) = " + _LRTable[buffer.state, GetColByID(buffer.sign)] + "\n";
            try
            {
                buffer = workStack.Peek();
            }
            catch (InvalidOperationException) { return; }
            buffer.sign = buf;
            loopCounter--;
        }

        private string getTheRule(int lex)
        {
            string buf = "";
            switch (lex - 1)
            {
                case 100:
                    outputString += "<P> -> S <PO> <POp> F $";
                    buf = "<P>";
                    popIt(5);
                    break;
                case 101:
                    outputString += "<PO> -> <O> ;";
                    buf = "<PO>";
                    popIt(2);
                    break;
                case 102:
                    outputString += "<PO> -> <O> ; <PO>";
                    buf = "<PO>";
                    popIt(3);
                    break;
                case 103:
                    outputString += "<POp> -> <Op> ;";
                    buf = "<POp>";
                    popIt(2);
                    break;
                case 104:
                    outputString += "<POp> -> <Op> ; <POp>";
                    buf = "<POp>";
                    popIt(3);
                    break;
                case 105:
                    outputString += "<O> -> d <LID>";
                    buf = "<O>";
                    popIt(2);
                    break;
                case 106:
                    outputString += "<O> -> B <LID>";
                    buf = "<O>";
                    popIt(2);
                    break;
                case 107:
                    outputString += "<LID> -> X ";
                    buf = "<LID>";
                    popIt(1);
                    break;
                case 108:
                    outputString += "<LID> -> X , <LID> ";
                    popIt(3);
                    buf = "<LID>";
                    break;
                case 109:
                    outputString += "<Op> -> X = <V>";
                    buf = "<Op>";
                    popIt(3);
                    break;
                case 110:
                    outputString += "<Op> -> W <V> D <POp> F";
                    buf = "<Op>";
                    popIt(5);
                    break;
                case 111:
                    outputString += "<Op> -> I <V> T <POp> E <POp> f";
                    buf = "<Op>";
                    popIt(7);
                    break;
                case 112:
                    outputString += "<Op> -> i X";
                    buf = "<Op>";
                    popIt(2);
                    break;
                case 113:
                    outputString += "<Op> -> o <V>";
                    buf = "<Op>";
                    popIt(2);
                    break;
                case 114:
                    outputString += "<V> -> <F>";
                    buf = "<V>";
                    popIt(1);
                    break;
                case 115:
                    outputString += "<V> -> <V> + <F>";
                    buf = "<V>";
                    popIt(3);
                    break;
                case 116:
                    outputString += "<V> -> <V> - <F>";
                    buf = "<V>";
                    popIt(3);
                    break;
                case 117:
                    outputString += "<F> -> <PV>";
                    buf = "<F>";
                    popIt(1);
                    break;
                case 118:
                    outputString += "<F> -> <F> * <PV>";
                    buf = "<F>";
                    popIt(3);
                    break;
                case 119:
                    outputString += "<F> -> <F> / <PV>";
                    buf = "<F>";
                    popIt(3);
                    break;
                case 120:
                    outputString += "<F> -> <F> % <PV>";
                    buf = "<F>";
                    popIt(3);
                    break;
                case 121:
                    outputString += "<PV> -> X";
                    buf = "<PV>";
                    popIt(1);
                    break;
                case 122:
                    outputString += "<PV> -> C";
                    buf = "<PV>";
                    popIt(1);
                    break;
                case 123:
                    outputString += "<PV> -> ( <V> )";
                    buf = "<PV>";
                    popIt(3);
                    break;
                case 124:
                    outputString += "<PV> -> ( <V> c <V> )";
                    buf = "<PV>";
                    popIt(5);
                    break;
            }
            outputString += "\n";
            return buf;
        }

        private void popIt(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                outputStack.Push(workStack.Pop());
            }
        }

    }



}
