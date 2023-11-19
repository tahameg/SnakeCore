using System;

namespace TahaCore.Runtime.Config.Types
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
    public class EnumConfigType<TEnumType> : ConfigType<TEnumType> where TEnumType : IConvertible
    {
        /// <inheritdoc cref="ConfigType{T}(string)"/>
        /// <exception cref="ArgumentException"> Thrown if TEnumType is not enum.</exception>
        public EnumConfigType(string value) : base(value)
        {
            if(!typeof(TEnumType).IsEnum)
                throw new ArgumentException("TEnumType must be an enumerated type");
        }
        
        /// <inheritdoc cref="ConfigType{T}()"/>
        /// <exception cref="ArgumentException"> Thrown if TEnumType is not enum.</exception>
        public EnumConfigType()
        {
            if(!typeof(TEnumType).IsEnum)
                throw new ArgumentException("TEnumType must be an enumerated type");
        }

        /// <inheritdoc cref="ConfigType{T}.Parse(string)" />
        protected override TEnumType Parse(string value)
        {
            if(value == null) return default;
            string trimmedValue = value.Trim();
            if(int.TryParse(value, out int intValue))
                return ParseAsValue(intValue);
            return ParseAsName(trimmedValue);
        }
        
        
        private TEnumType ParseAsValue(int value)
        {
            if(!Enum.IsDefined(typeof(TEnumType), value))
                throw new FormatException($"Could not parse value to Enum Type since it is out of scope: {value}");
            return (TEnumType)Enum.ToObject(typeof(TEnumType), value);
        }
        
        private TEnumType ParseAsName(string value)
        {
            return (TEnumType)Enum.Parse(typeof(TEnumType), value);
        }
    }
}