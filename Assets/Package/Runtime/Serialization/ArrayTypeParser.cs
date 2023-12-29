using System;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    [Preserve]
    internal sealed class ArrayTypeParser : ITypeParser
    {
        private readonly ITypeParser m_elementTypeParser;
        private readonly Type m_elementType;
        public Type TargetType { get;}
        public bool CanBeArrayElement { get; } = false;

        internal ArrayTypeParser(ITypeParser elementTypeParser)
        {
            m_elementType = elementTypeParser.TargetType;
            m_elementTypeParser = elementTypeParser;
            TargetType = m_elementType.MakeArrayType();
        }

        /// <summary>
        /// Parses given string value to an array of TArrayType. Value will be set to default if the given string is null.
        /// </summary>
        /// <param name="value">String value to be parsed. </param>
        /// <returns>An array of parsed elements. Returns default value if the given string is null.</returns>
        /// <exception cref="FormatException">Thrown if the given string cannot be parsed.</exception>
        public object Parse(string value)
        {
            if(value == null) return default;
            var trimmedString = value.Trim();
            
            if (string.IsNullOrEmpty(trimmedString))
                throw new FormatException("Value cannot be empty.");
            
            if (!trimmedString.StartsWith('[') || !trimmedString.EndsWith(']'))
                throw new FormatException("Value must be enclosed in square brackets.");
            
            var elements = trimmedString.Substring(1, trimmedString.Length - 2).Split(',');

            var returnArray
                = Array.CreateInstance(m_elementType, elements.Length);
            for(int i = 0; i < returnArray.Length; i++)
            {
                returnArray.SetValue(ParseItem(elements[i]), i);
            }
            return returnArray;
        }

        /// <summary>
        /// Parses given string value to TArrayType.
        /// </summary>
        /// <param name="value">String value to be parsed.</param>
        /// <returns></returns>
        /// <exception cref="FormatException">Thrown if the given string cannot be parsed.</exception>
        private object ParseItem(string value)
        {
            return m_elementTypeParser.Parse(value);
        }
    }
}