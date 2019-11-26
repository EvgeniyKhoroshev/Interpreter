namespace Interpreter.Translation
{
    /// <summary>
    /// Перечисление возможных лексических токенов.
    /// </summary>
    public enum TranslationToken
    {
        /// <summary>
        /// _.
        /// </summary>
        Space = 0,

        /// <summary>
        /// X.
        /// </summary>
        Identifier = 1,

        /// <summary>
        /// B.
        /// </summary>
        BooleanDataType = 2,

        /// <summary>
        /// d.
        /// </summary>
        IntDataType = 3,

        /// <summary>
        /// R.
        /// </summary>
        BreakKeyword = 4,

        /// <summary>
        /// I.
        /// </summary>
        IfKeyword = 5,

        /// <summary>
        /// W.
        /// </summary>
        WhileKeyword = 6,

        /// <summary>
        /// i.
        /// </summary>
        InputKeyword = 7,

        /// <summary>
        /// e.
        /// </summary>
        EchoKeyword = 8,

        /// <summary>
        /// C.
        /// </summary>
        Constant = 9,

        /// <summary>
        /// E.
        /// </summary>
        ElseKeyword = 10,

        /// <summary>
        /// c.
        /// </summary>
        ComparsionOpearation = 11,

        /// <summary>
        /// ;.
        /// </summary>
        Semicolon = 12,

        /// <summary>
        /// ,.
        /// </summary>
        Comma = 13,

        /// <summary>
        /// {.
        /// </summary>
        LeftBrace = 14,

        /// <summary>
        /// }.
        /// </summary>
        RightBrace = 15,

        /// <summary>
        /// (.
        /// </summary>
        LeftParentheses = 16,

        /// <summary>
        /// ).
        /// </summary>
        RightParentheses = 17,

        /// <summary>
        /// =.
        /// </summary>
        AssignOperation = 18,

        /// <summary>
        /// +.
        /// </summary>
        PlusOperation = 19,

        /// <summary>
        /// -.
        /// </summary>
        MinusOperation = 20,

        /// <summary>
        /// *.
        /// </summary>
        MultipleOperation = 21,

        /// <summary>
        /// /.
        /// </summary>
        DivisionOperation = 22,

        /// <summary>
        /// %.
        /// </summary>
        RemainderOfTheDivisionOperation = 23,

        /// <summary>
        /// $.
        /// </summary>
        EndOfProgram = 24,

        /// <summary>
        /// T.
        /// </summary>
        Triada = 25,

        /// <summary>
        /// :.
        /// </summary>
        GotoLabel = 26,

        /// <summary>
        /// >.
        /// </summary>
        GotoTransition = 27,
    }
}
