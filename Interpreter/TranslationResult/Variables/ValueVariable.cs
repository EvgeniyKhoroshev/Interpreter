namespace Interpreter.TranslationResult.Variables
{
    /// <summary>
    /// Абстрактный класс для хранение информации о переменных.
    /// </summary>
    /// <typeparam name="TValueType">Тип данных.</typeparam>
    public abstract class Variable<TValueType>
        where TValueType : struct
    {
        /// <summary>
        /// Тип данных.
        /// </summary>
        public virtual DataType Type { get; protected set; }

        /// <summary>
        /// Значение переменной.
        /// </summary>
        public virtual TValueType Value { get; set; }

        /// <summary>
        /// Название переменной.
        /// </summary>
        public virtual string Name { get; set; }
    }
}
