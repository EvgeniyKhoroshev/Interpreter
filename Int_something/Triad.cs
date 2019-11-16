using System;
using System.Collections.Generic;

namespace Int_something
{
    class Triad : PostfixNotation
    {
        public struct Triada
        {
            public int triadNumber;
            public TranslatedToken FirstOperand;
            public TranslatedToken SecondOperand;
            public TranslatedToken Operation;
        }
        public Triada buf;
        int triadCounter = 0;
        Stack<TranslatedToken> workStack;
        public Triada[] ThreeAddressCode;
        private Queue<Triada> ThreeAddressCodeQueue;
        public void ProcessTriads()
        {
            buf = new Triada();
            workStack = new Stack<TranslatedToken>();
            ThreeAddressCodeQueue = new Queue<Triada>();
            this.ClearBuffer();
            while (this.output.Count>0)
            {
                this.Buffer = this.output.Dequeue();
                ActionCase();
            }
            ThreeAddressCode = ThreeAddressCodeQueue.ToArray();
        }
        void createTriad()
        {
            this.ClearBuffer();
            this.Buffer.Value = "T" + Convert.ToString(triadCounter);
            this.Buffer.Token = 'T';
            this.Buffer.numberInProgram = triadCounter;
        }
        private void ActionCase()
        {
            switch (this.Buffer.Token)
            {
                case 'i':
                case 'e':
                    buf.Operation = this.Buffer;
                    this.ClearBuffer();
                    buf.FirstOperand = workStack.Pop();
                    buf.triadNumber = triadCounter;
                    createTriad();
                    ThreeAddressCodeQueue.Enqueue(buf);
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
                    buf.Operation = this.Buffer;
                    this.ClearBuffer();
                    buf.SecondOperand = workStack.Pop();
                    buf.FirstOperand = workStack.Pop();
                    buf.triadNumber = triadCounter;
                    ThreeAddressCodeQueue.Enqueue(buf);
                    createTriad();
                    workStack.Push(this.Buffer);
                    clearBuf();
                    ++triadCounter;
                    break;
                case ':':
                case '>':
                    buf.Operation = this.Buffer;
                    if (this.Buffer.isConditionalBranch)
                        buf.FirstOperand = workStack.Pop();
                    this.ClearBuffer();
                    buf.triadNumber = triadCounter;
                    createTriad();
                    ThreeAddressCodeQueue.Enqueue(buf);
                    clearBuf();
                    ++triadCounter;
                    break;
            }
        }

        public void clearBuf()
        {
            this.ClearBuffer();
            this.buf.Operation = this.Buffer;
            this.buf.FirstOperand = this.Buffer;
            this.buf.SecondOperand = this.Buffer;
        }
    }
}
