// =======================================================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Parses a string value to string. Use this to parse a string value to string.<br/>
    /// </summary>
    [TypeParserContextRegistry]
    public class StringTypeParser : ITypeParser
    {
        public Type TargetType { get; } = typeof(string);
        public bool CanBeArrayElement { get; } = true;
        
        /// <summary>
        /// Parses the given string value to string. If the value is null, string.Empty is returned.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <returns>Returns the trimmed value.</returns>
        public object Parse(string value)
        {
            return value.Trim();
        }
    }
}