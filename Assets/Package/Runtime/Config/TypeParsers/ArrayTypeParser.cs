// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System;
using TahaCore.Serialization;
using TahaCore.Serialization.TypeParsers;
using UnityEngine.Scripting;

namespace TahaCore.Config.TypeParsers
{
    /// <summary>
    /// Parses a string value to an array of objects. The string should be wrapped between
    /// square brackets and elements should be separated by commas.<br/>
    /// <br/>
    /// Type of the array is determined by the given elementTypeParser parameter. For example: <br/>
    /// When elementTypeParser is a DoubleTypeDeserializer, the array will be an array of doubles.<br/>
    /// </summary>
    [ConfigTypeParser]
    [Preserve]
    internal sealed class ArrayTypeParser : ITypeParser
    {
        public Type TargetType { get;}
        private readonly Type m_elementType;

        /// <summary>
        /// Creates a new ArrayTypeParser with the given elementTypeParser.
        /// </summary>
        /// <param name="elementType">Type of the elements of this array.</param>
        internal ArrayTypeParser(Type elementType)
        {
            m_elementType = elementType;
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
            // if value is null, return default
            if(value == null) return default;
            
            // trim the value
            var trimmedString = value.Trim();
            
            // if the value is empty, return default
            if (string.IsNullOrEmpty(trimmedString))
            {
                return default;   
            }

            // if the value doesn't start with '[' or doesn't end with ']', throw exception
            if (!trimmedString.StartsWith('[') || !trimmedString.EndsWith(']'))
            {
                throw new FormatException("Value must be enclosed in square brackets.");
            }
                
            // trim the square brackets and split the elements
            var elements = trimmedString.Substring(1, trimmedString.Length - 2).Split(',');

            var returnArray = Array.CreateInstance(m_elementType, elements.Length);
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
            if(!CommonTypeParser.TryParsePrimitive(m_elementType, value, out var parsedValue))
            {
                throw new FormatException($"Cannot parse '{value}' to {m_elementType.Name}.");
            }
            return parsedValue;
        }
    }
}