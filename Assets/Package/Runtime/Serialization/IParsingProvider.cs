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
    public interface IParsingProvider
    {
        /// <summary>
        /// Parse string value to the given type.
        /// </summary>
        /// <param name="targetType">Type to parser the value to.</param>
        /// <param name="value">String value to parser.</param>
        /// <returns>Parsed value of the given string.</returns>
        object Parse(Type targetType, string value);
        
        /// <summary>
        /// Parse string value to the given type.
        /// </summary>
        /// <param name="value">String value to parser.</param>
        /// <typeparam name="T">Type to parser the value to.</typeparam>
        /// <returns>Parsed value of the given string.</returns>
        T Parse<T>(string value);
        
        bool CanParse(Type targetType);
    }
}