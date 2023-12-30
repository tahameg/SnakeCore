// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Parser for parsing double precision floating point numbers. Use this to parse a string value to Double.<br/>
    /// Data format is as follows: 1.0 or 1
    /// </summary>
    [TypeParserContextRegistry]
    [Preserve]
    internal class DoubleTypeParser : ITypeParser
    {
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = true;
        
        /// <summary>
        /// Creates a new instance of DoubleTypeParser.
        /// </summary>
        internal DoubleTypeParser()
        {
            TargetType = typeof(double);
        }
        
        /// <summary>
        /// Parses the given string value to Double. If the value is null, 0 is returned.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <returns>Double value of the given string.</returns>
        /// <exception cref="FormatException">If the given string is not a valid Double.</exception>
        public object Parse(string value)
        {
            if(value == null) return default;
            try
            {
                return Convert.ToDouble(value.Trim());
            }
            catch (Exception)
            {
                throw new FormatException("Could not parse value to Double");
            }
        }
    }
}