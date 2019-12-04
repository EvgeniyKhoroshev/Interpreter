using Newtonsoft.Json;
using System;
using System.IO;

namespace Interpreter.Extensions
{
    /// <summary>
    /// Расширения для работы с файлами.
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// Записывает объект <paramref name="source"/> в файл <paramref name="path"/>.
        /// </summary>
        public static void WriteToFile<T>(this T source, string path, bool append = false)
        {
            var jsonSerializer = new JsonSerializer();
            try
            {
                using (StreamWriter file = new StreamWriter(path, append))
                {
                    jsonSerializer.Serialize(file, source);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"An error occured while writing " +
                    $"{source.GetType().ToString()} to file {path}.", ex);
            }
        }

        /// <summary>
        /// Считывает объект <paramref name="source"/> из файла <paramref name="path"/>.
        /// </summary>
        public static void ReadFromFile<T>(this T source, string path)
            => _ = (T)ReadFromFile(source.GetType(), path);

        /// <summary>
        /// Возвращает объект с типом <paramref name="type"/> считанный из файла <paramref name="path"/>.
        /// </summary>
        public static object ReadFromFile(Type type, string path)
        {
            var json = new JsonSerializer();
            object result = default;

            try
            {
                using (StreamReader file = new StreamReader(path, true))
                    result = json.Deserialize(file, type);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured while reading {type.ToString()} from file {path}.", ex);
            }

            return result;
        }
    }
}
