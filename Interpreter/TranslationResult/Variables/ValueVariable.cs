namespace Interpreter.TranslationResult.Variables
{
    /// <summary>
    /// Абстрактный класс для хранение информации о переменных.
    /// </summary>
    /// <typeparam name="TDataType">Тип данных.</typeparam>
    public class ValueVariable<TDataType>
        where TDataType : struct
    {
        /// <summary>
        /// Тип данных.
        /// </summary>
        public virtual DataType Type { get; protected set; } 

        /// <summary>
        /// Значение переменной.
        /// </summary>
        public virtual TDataType Value { get; set; }

        /// <summary>
        /// Название переменной.
        /// </summary>
        public virtual string Name { get; set; }
    }
}
