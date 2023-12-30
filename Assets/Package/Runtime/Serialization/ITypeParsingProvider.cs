// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Interface for parsing string values to a given type.
    /// </summary>
    public interface ITypeParsingProvider
    {
        /// <summary>
        /// Parse string value to the given type.
        /// </summary>
        /// <param name="targetType">Type to parse the value to.</param>
        /// <param name="value">String value to parse.</param>
        /// <returns>Parsed value of the given string.</returns>
        object Parse(Type targetType, string value);
        
        /// <summary>
        /// Parse string value to the given type.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <typeparam name="T">Type to parse the value to.</typeparam>
        /// <returns>Parsed value of the given string.</returns>
        T Parse<T>(string value);
    }
}