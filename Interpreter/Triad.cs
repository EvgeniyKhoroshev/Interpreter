using Interpreter.Translation;
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
            Buffer.Value = Convert.ToString(triadCounter);
            Buffer.Token = TranslationToken.Triada;
            Buffer.LineNumber = triadCounter;
        }
        private void ActionCase()
        {
            switch (Buffer.Token)
            {
                case TranslationToken.InputKeyword:
                case TranslationToken.EchoKeyword:
                    BufferTriada.Operation = Buffer;
                    Buffer.Clear();
                    BufferTriada.FirstOperand = workStack.Pop();
                    BufferTriada.TriadNumber = triadCounter;
                    createTriad();
                    ThreeAddressCodeQueue.Enqueue(BufferTriada);
                    clearBuf();
                    ++triadCounter;
                    break;
                case TranslationToken.Identifier:
                case TranslationToken.Constant:
                    workStack.Push(Buffer);
                    break;
                case TranslationToken.PlusOperation:
                case TranslationToken.MinusOperation:
                case TranslationToken.DivisionOperation:
                case TranslationToken.MultipleOperation:
                case TranslationToken.RemainderOfTheDivisionOperation:
                case TranslationToken.AssignOperation:
                case TranslationToken.ComparsionOpearation:
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
                case TranslationToken.GotoLabel:
                case TranslationToken.GotoTransition:
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
