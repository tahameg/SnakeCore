// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Container for DateTime. Use this to parse a string value to DateTime.<br/>
    /// </summary>
    [TypeParserContextRegistry]
    [Preserve]
    internal class DateTimeTypeParser : ITypeParser
    {
        
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = true;
        
        /// <summary>
        /// Creates a new instance of DateTimeTypeParser.
        /// </summary>
        internal DateTimeTypeParser()
        {
            TargetType = typeof(DateTime);
        }
        
        /// <summary>
        /// Parses the given string value to DateTime. If the value is null, default DateTime is returned.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <returns>DateTime value of the given string.</returns>
        /// <exception cref="FormatException">If the given string is not a valid DateTime.</exception>
        public object Parse(string value)
        {
            try
            {
                if (value == null) return default;
                string trimmedValue = value.Trim();
                return DateTime.Parse(trimmedValue);
            }
            catch (Exception)
            {
                throw new FormatException("Could not parse value to DateTime");
            }
        }
    }
}