using Interpreter.TranslationResult;
using System;
using System.Collections.Generic;

namespace Interpreter
{
    class Triad : PostfixNotation
    {
        private int triadCounter = 0;
        LexicalToken Buffer = new LexicalToken();
        public Triada BufferTriada;
        Stack<LexicalToken> workStack;
        public Triada[] ThreeAddressCode;
        private Queue<Triada> ThreeAddressCodeQueue;
        public void ProcessTriads()
        {
            BufferTriada = new Triada();
            workStack = new Stack<LexicalToken>();
            ThreeAddressCodeQueue = new Queue<Triada>();
            Buffer.Clear();
            while (output.Count > 0)
            {
                Buffer = output.Dequeue();
                ActionCase();
            }
            ThreeAddressCode = ThreeAddressCodeQueue.ToArray();
        }
        void createTriad()
        {
            Buffer.Clear();
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
                    Buffer.Clear();
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
                    Buffer.Clear();
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
                    Buffer.Clear();
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
            Buffer.Clear();
            BufferTriada.Operation = Buffer;
            BufferTriada.FirstOperand = Buffer;
            BufferTriada.SecondOperand = Buffer;
        }
    }
}
