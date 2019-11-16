using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int_something
{
    class Triad : toPostfix
    {
        public struct Triada
        {
            public int triadNumber;
            public translationTable operator_1;
            public translationTable operator_2;
            public translationTable operation;
        }
        public Triada buf;
        int triadCounter = 0;
        Stack<translationTable> workStack;
        public Triada[] T_AC;
        private Queue<Triada> Three_addressCode;
        public void doTriad()
        {
            buf = new Triada();
            workStack = new Stack<translationTable>();
            Three_addressCode = new Queue<Triada>();
            this.ClearBuffer();
            while (this.output.Count>0)
            {
                this.Buffer = this.output.Dequeue();
                actionCase();
            }
            T_AC = Three_addressCode.ToArray();
        }
        void createTriad()
        {
            this.ClearBuffer();
            this.Buffer.Value = "T" + Convert.ToString(triadCounter);
            this.Buffer.Token = 'T';
            this.Buffer.numberInProgram = triadCounter;
        }
        private void actionCase()
        {
            switch (this.Buffer.Token)
            {
                case 'i':
                case 'e':
                    buf.operation = this.Buffer;
                    this.ClearBuffer();
                    buf.operator_1 = workStack.Pop();
                    buf.triadNumber = triadCounter;
                    createTriad();
                    Three_addressCode.Enqueue(buf);
                    clearBuf();
                    ++triadCounter;
                    break;
                case 'X':
                case 'C':
                    workStack.Push(this.Buffer);
                    break;
                case '+':
                case '-':
                case '/':
                case '*':
                case '%':
                case '=':
                case 'c':
                    buf.operation = this.Buffer;
                    this.ClearBuffer();
                    buf.operator_2 = workStack.Pop();
                    buf.operator_1 = workStack.Pop();
                    buf.triadNumber = triadCounter;
                    Three_addressCode.Enqueue(buf);
                    createTriad();
                    workStack.Push(this.Buffer);
                    clearBuf();
                    ++triadCounter;
                    break;
                case ':':
                case '>':
                    buf.operation = this.Buffer;
                    if (this.Buffer.isConditionalBranch)
                        buf.operator_1 = workStack.Pop();
                    this.ClearBuffer();
                    buf.triadNumber = triadCounter;
                    createTriad();
                    Three_addressCode.Enqueue(buf);
                    clearBuf();
                    ++triadCounter;
                    break;
            }
        }
        public void clearBuf()
        {
            this.ClearBuffer();
            this.buf.operation = this.Buffer;
            this.buf.operator_1 = this.Buffer;
            this.buf.operator_2 = this.Buffer;
        }
    }
}
