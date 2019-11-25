namespace Interpreter.TranslationResult
{
    public class Triada
    {
        public Triada()
        {
            FirstOperand = new LexicalToken();
            SecondOperand = new LexicalToken();
            Operation = new LexicalToken(); 
        }

        public int TriadNumber;
        public LexicalToken FirstOperand;
        public LexicalToken SecondOperand;
        public LexicalToken Operation;
    }

    public static class TriadaExtensions
    {
        public static void Clear(this Triada source)
        {
            source.FirstOperand.Clear();
            source.SecondOperand.Clear();
            source.Operation.Clear();
        }
    }
}
