// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Parser for parsing Guid from string. Use this to parse a string value to Guid.
    /// </summary>
    [TypeParserContextRegistry]
    [Preserve]
    internal class GuidTypeParser : ITypeParser
    {
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = true;

        /// <summary>
        /// Creates a new instance of GuidTypeParser.
        /// </summary>
        internal GuidTypeParser()
        {
            TargetType = typeof(Guid);
        }
        
        /// <summary>
        /// Parses the given string value to Guid. If the value is null, Guid.empty is returned.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <returns>Guid value of the given string.</returns>
        public object Parse(string value)
        {
            if(value == null) return default;
            string trimmedValue = value.Trim();
            return Guid.Parse(trimmedValue);
        }
    }
}