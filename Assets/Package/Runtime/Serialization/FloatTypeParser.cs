// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Parser for parsing Single precision floating point numbers from string.
    /// Use this to parse a config value to Single.<br/>
    /// Data format is as follows: 1.0 or 1 <br/>
    /// Do not use f postfix.
    /// </summary>
    [TypeParserContextRegistry]
    [Preserve]
    internal class FloatTypeParser : ITypeParser
    {
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = true;

        /// <summary>
        /// Creates a new instance of FloatTypeParser.
        /// </summary>
        internal FloatTypeParser()
        {
            TargetType = typeof(float);
        }
        
        /// <summary>
        /// Parses the given string value to Single. If the value is null, 0 is returned.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <returns>Single value of the given string.</returns>
        /// <exception cref="FormatException">If the given string is not a valid Single.</exception>
        public object Parse(string value)
        {
            if(value == null) return default;
            try
            {
                return Convert.ToSingle(value.Trim());
            }
            catch (Exception)
            {
                throw new FormatException("Could not parse value to Single");
            }
        }
    }
}