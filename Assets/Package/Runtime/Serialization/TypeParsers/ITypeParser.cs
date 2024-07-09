// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System;

namespace SnakeCore.Serialization.TypeParsers
{
    /// <summary>
    /// Interface for parsing data types from string. Each TypeParser targets a specific type.
    /// <see cref="ITypeParserLocator"/>
    /// </summary>
    public interface ITypeParser
    {
        /// <summary>
        /// Should return the type that this parser can parser.
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// Parse string value to generic type.
        /// </summary>
        /// <param name="value">String value to parser.</param>
        /// <returns>The parsed value. Returns default if the given value is null.</returns>
        /// <exception cref="FormatException">Thrown if the value cannot be parsed to type.</exception>
        object Parse(string value);
    }
}