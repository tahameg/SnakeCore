using System;
using System.Diagnostics.Contracts;

namespace TahaCore.Serialization
{
    public interface ITypeParser
    {
        Type TargetType { get; }
        bool CanBeArrayElement { get; }
        
        /// <summary>
        /// Parse string value to generic type.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <returns>The parsed value. Returns default if the given value is null.</returns>
        /// <exception cref="FormatException">Thrown if the value cannot be parsed to type.</exception>
        object Parse(string value);
    }
}