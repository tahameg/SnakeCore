// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;

namespace TahaCore.Serialization
{
    /// <summary>
    /// TypeParserContext is used to get parsers for types.
    /// </summary>
    internal interface ITypeParserContext
    {
        /// <summary>
        /// Get parser for the given type.
        /// </summary>
        /// <param name="targetType">Type to get parser for.</param>
        /// <returns>Parser for the given type.</returns>
        /// <exception cref="ArgumentNullException">If the given type is null.</exception>
        /// <exception cref="ArgumentException">If no parser is found for the given type.</exception>
        ITypeParser GetParserForType(Type targetType);

        /// <summary>
        /// Get parser of type T.
        /// </summary>
        /// <typeparam name="T">Type of parser to get.</typeparam>
        /// <returns>Parser of type T. Null if no parser was found.</returns>
        ITypeParser GetParser<T>() where T : ITypeParser;
    }
}