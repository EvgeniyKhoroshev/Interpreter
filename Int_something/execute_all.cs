using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int_something
{
    class execute_all : Execution
    {
        public string toOut;
        public int tokens_count = 0;
        public LA lex;
        public SA_LL1 synt;
        TranslationTable some, toS;
        public execute_all()
        {
        }

        public void solve(string[] inputCode)
        {
            this.source = new TranslationTable();
            lex = new LA(inputCode);
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
            toOut = "";
            some = lex.doAnalyze();
            if (lex.ErrorListLA.Count > 0)
                return;
            toSA();
            synt = new SA_LL1(toS.TranslationList);
            if (synt.errLog.Count > 0)
                return;
            synt.Do();
            this.regroup();
            toOut += "\n";
            foreach (translationTable t in source.TranslationList)
                toOut += t + "\n";
            this.postfixRecord();
            this.doTriad();
            exec();
        }
        void toSA()
        {
            toOut = "Список токенов:\n";
            foreach (translationTable b in some.TranslationList)
            {
                if (b.Token != '_')
                {
                    toOut += b.Token;
                    toS.TranslationList.Enqueue(b);
                }
            }
            tokens_count = toS.TranslationList.Count;
            this.source = some;
        }
    }
}
