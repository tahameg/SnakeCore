using System;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Container for Enum Types. Use this to parse a config value to Enum Type. Both enum value and name are accepted.
    /// Consider an example enum as:
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
    /// <typeparam name="TEnumType">Type of enum</typeparam>
    [Preserve]
    internal class EnumTypeParser : ITypeParser
    {
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = true;

        /// <inheritdoc cref="ConfigType{T}(string)"/>
        /// <exception cref="ArgumentException"> Thrown if TEnumType is not enum.</exception>
        public EnumTypeParser(Type enumType)
        {
            if(!enumType.IsEnum)
                throw new ArgumentException("TEnumType must be an enumerated type");
            TargetType = enumType;
        }

        /// <inheritdoc cref="ConfigType{T}.Parse(string)" />
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
            if(!Enum.IsDefined(TargetType, value))
                throw new FormatException($"Could not parse value to Enum Type since it is out of scope: {value}");
            return Enum.ToObject(TargetType, value);
        }
        
        private object ParseAsName(string value)
        {
            return Enum.Parse(TargetType, value);
        }
    }
}