// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Parser for parsing Integer numbers from string. Use this to parse a string value to Integer.
    /// </summary>
    [TypeParserContextRegistry]
    [Preserve]
    internal class IntegerTypeParser : ITypeParser
    {
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = true;

        /// <summary>
        /// creates a new instance of IntegerTypeParser.
        /// </summary>
        internal IntegerTypeParser()
        {
            TargetType = typeof(int);
        }

        /// <summary>
        /// Parses the given string value to Integer. If the value is null, 0 is returned.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <returns>Integer value of the given string.</returns>
        /// <exception cref="FormatException">If the given string is not a valid Integer.</exception>
        public object Parse(string value)
        {
            if(value == null) return default;
            try
            {
                return Convert.ToInt32(value.Trim());
            }
            catch (Exception)
            {
                throw new FormatException("Could not parse value to Integer");
            }
        }
    }
}