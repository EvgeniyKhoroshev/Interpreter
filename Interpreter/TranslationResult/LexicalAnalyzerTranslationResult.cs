using System.Collections.Generic;

namespace Interpreter.TranslationResult
{
    public class LexicalAnalyzerTranslationResult
    {
        public TranslationTable Table { get; set; }
        public List<string> Errors { get; set; }
        public string StateLog { get; set; }
    }
}
