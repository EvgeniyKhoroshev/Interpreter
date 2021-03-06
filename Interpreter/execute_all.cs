﻿using Interpreter.Translation;
using Interpreter.TranslationResult;
using System;

namespace Interpreter
{
    class execute_all : Execution
    {
        public string toOut;
        public int tokens_count = 0;
        public LA LexicalAnalyser;
        public SA_LL1 SyntaxAnalyser;
        TranslationTable some, toS;
        public execute_all()
        {
        }

        public void solve(string[] inputCode)
        {
            this.source = new TranslationTable();
            LexicalAnalyser = new LA(inputCode);
            toS = new TranslationTable();
            some = new TranslationTable();
            try
            {
                Init();
            }
            catch (Exception) { }
        }
        void Init()
        {
            some = LexicalAnalyser.TranslateCode();
            if (LexicalAnalyser.ErrorListLA.Count > 0)
                return;
            toSA();
            SyntaxAnalyser = new SA_LL1(toS.TranslationList);
            if (SyntaxAnalyser.errLog.Count > 0)
                return;
            SyntaxAnalyser.Do();
            this.regroup();
            toOut += "\n";
            foreach (LexicalToken t in source.TranslationList)
                toOut += t + "\n";
            this.postfixRecord();
            this.ProcessTriads();
            exec();
        }
        void toSA()
        {
            toOut = "Список токенов:\n";
            foreach (LexicalToken b in some.TranslationList)
            {
                if (b.Token != TranslationToken.Space)
                {
                    toOut += b.Token;
                    toS.TranslationList.Enqueue(b);
                }
            }

            tokens_count = toS.TranslationList.Count;
            source = some;
        }
    }
}
