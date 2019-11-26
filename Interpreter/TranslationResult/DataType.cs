namespace Interpreter.TranslationResult
{
    /// <summary>
    /// Перечисление типов данных.
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// Значение не определено.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Целочисленный.
        /// </summary>
        Int = 1,

        /// <summary>
        /// Логический.
        /// </summary>
        Bool = 2,

        /// <summary>
        /// Строковый.
        /// </summary>
        String = 3,
    }
}
