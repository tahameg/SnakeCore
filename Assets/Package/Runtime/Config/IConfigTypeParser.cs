using System;

namespace TahaCore.Runtime.Config
{
    public interface IConfigTypeParser
    {
        /// <summary>
        /// Parses the given string value to the given type.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <typeparam name="T">Target type.</typeparam>
        /// <returns>Parsed value.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the given type is not supported.</exception>
        T Parse<T>(string value);
    }
}