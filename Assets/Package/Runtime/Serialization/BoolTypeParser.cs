// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Parses a string value to bool. Use this to parse a string value to bool.<br/>
    /// The following values are considered true: "1", "True" (Case insensitive)<br/>
    /// The following values are considered false: "0", "False" (Case insensitive)<br/> 
    /// </summary>
    [TypeParserContextRegistry]
    [Preserve]
    internal class BoolTypeParser : ITypeParser
    {
        
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = true;
        
        /// <summary>
        /// Creates a new instance of BoolTypeParser.
        /// </summary>
        internal BoolTypeParser()
        {
            TargetType = typeof(bool);
        }

        /// <summary>
        /// Parses the given string value to bool. If the value is null, false is returned.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <returns>True if value is "1" or "True". False if value is "0" or "False".</returns>
        /// <exception cref="FormatException">Thrown the value doesn't have one of the given values.</exception>
        public object Parse(string value)
        {
            if (value == null) return false;
            string fixedValue = value.Trim().ToLower();
            if (fixedValue == "true") return true;
            if (fixedValue == "false") return false;
            if (fixedValue == "1") return true;
            if (fixedValue == "0") return false;
            throw new FormatException($"Invalid bool value: {value}");
        }
    }
}