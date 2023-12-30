// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Parser for parsing Enum Types. Use this to parse a config value to Enum Type.
    /// Both enum value and name are accepted. Consider an example enum as:
    /// <code>
    /// enum ExampleEnum
    /// {
    ///     Value1 = 1,
    ///     Value2 = 2,
    /// }
    /// </code>
    /// Values are parsed as follows:
    /// <code>
    /// EnumConfigType&lt;ExampleEnum&gt;("Value1").Value; // returns ExampleEnum.Value1
    /// EnumConfigType&lt;ExampleEnum&gt;("1").Value; // returns ExampleEnum.Value1
    /// </code>
    /// </summary>
    [TypeParserContextRegistry]
    internal class EnumTypeParser : ITypeParser
    {
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = true;

        /// <summary>
        /// Creates a new instance of EnumTypeParser.
        /// </summary>
        /// <param name="enumType">Type of the enum. </param>
        /// <exception cref="ArgumentException">If the given type is not an enum type.</exception>
        public EnumTypeParser(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("TEnumType must be an enumerated type");   
            }
            TargetType = enumType;
        }
        
        /// <summary>
        /// Parses the given string value to Enum Type. If the value is null, default value of the enum is returned.
        /// The value can be either the name or the value of the enum.
        /// </summary>
        /// <param name="value">Value to parse.</param>
        /// <returns>The enum value of the given string.</returns>
        /// Throws FormatException if the given string is not a valid enum value.
        public object Parse(string value)
        {
            if(value == null) return default;
            string trimmedValue = value.Trim();
            if (int.TryParse(value, out int intValue))
            {
                return ParseAsValue(intValue);   
            }
            return ParseAsName(trimmedValue);
        }
        
        private object ParseAsValue(int value)
        {
            if (!Enum.IsDefined(TargetType, value))
            {
                throw new FormatException($"Could not parse value to Enum Type since it is out of scope: {value}");   
            }
            return Enum.ToObject(TargetType, value);
        }
        
        private object ParseAsName(string value)
        {
            return Enum.Parse(TargetType, value);
        }
    }
}