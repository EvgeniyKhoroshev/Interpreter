using Int_something.TranslationResult;
using System;
using System.Collections.Generic;

namespace Int_something
{
    class Triad : PostfixNotation
    {
        private int triadCounter = 0;

        public struct Triada
        {
            public int TriadNumber;
            public LexicalToken FirstOperand;
            public LexicalToken SecondOperand;
            public LexicalToken Operation;
        }



        public Triada BufferTriada;
        Stack<LexicalToken> workStack;
        public Triada[] ThreeAddressCode;
        private Queue<Triada> ThreeAddressCodeQueue;
        public void ProcessTriads()
        {
            BufferTriada = new Triada();
            workStack = new Stack<LexicalToken>();
            ThreeAddressCodeQueue = new Queue<Triada>();
            ClearBuffer();
            while (output.Count > 0)
            {
                Buffer = output.Dequeue();
                ActionCase();
            }
            ThreeAddressCode = ThreeAddressCodeQueue.ToArray();
        }
        void createTriad()
        {
            ClearBuffer();
            Buffer.Value = "T" + Convert.ToString(triadCounter);
            Buffer.Token = 'T';
            Buffer.numberInProgram = triadCounter;
        }
        private void ActionCase()
        {
            switch (Buffer.Token)
            {
                case 'i':
                case 'e':
                    BufferTriada.Operation = Buffer;
                    ClearBuffer();
                    BufferTriada.FirstOperand = workStack.Pop();
                    BufferTriada.TriadNumber = triadCounter;
                    createTriad();
                    ThreeAddressCodeQueue.Enqueue(BufferTriada);
                    clearBuf();
                    ++triadCounter;
                    break;
                case 'X':
                case 'C':
                    workStack.Push(Buffer);
                    break;
                case '+':
                case '-':
                case '/':
                case '*':
                case '%':
                case '=':
                case 'c':
                    BufferTriada.Operation = Buffer;
                    ClearBuffer();
                    BufferTriada.SecondOperand = workStack.Pop();
                    BufferTriada.FirstOperand = workStack.Pop();
                    BufferTriada.TriadNumber = triadCounter;
                    ThreeAddressCodeQueue.Enqueue(BufferTriada);
                    createTriad();
                    workStack.Push(Buffer);
                    clearBuf();
                    ++triadCounter;
                    break;
                case ':':
                case '>':
                    BufferTriada.Operation = Buffer;
                    if (Buffer.isConditionalBranch)
                        BufferTriada.FirstOperand = workStack.Pop();
                    ClearBuffer();
                    BufferTriada.TriadNumber = triadCounter;
                    createTriad();
                    ThreeAddressCodeQueue.Enqueue(BufferTriada);
                    clearBuf();
                    ++triadCounter;
                    break;
            }
        }

        public void clearBuf()
        {
            ClearBuffer();
            BufferTriada.Operation = Buffer;
            BufferTriada.FirstOperand = Buffer;
            BufferTriada.SecondOperand = Buffer;
        }
    }
}
