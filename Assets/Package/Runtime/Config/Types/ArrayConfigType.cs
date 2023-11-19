using System;

namespace TahaCore.Runtime.Config.Types
{
    /// <summary>
    /// Base class for config types that can be represented as an array of values.
    /// </summary>
    /// <typeparam name="TArrayType">Type of the array.</typeparam>
    public abstract class ArrayConfigType<TArrayType> : ConfigType<TArrayType[]>
    {
        /// <summary>
        /// Create a new instance of ArrayConfigType with given value parsed.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// Format exception is 
        /// <exception cref="FormatException">Thrown if the given string cannot be parsed.</exception>
        protected ArrayConfigType(string value) : base(value){}
        
        /// <summary>
        /// Create a new instance of ArrayConfigType with default value.
        /// </summary>
        protected ArrayConfigType(){}
        
        /// <summary>
        /// Parses given string value to an array of TArrayType. Value will be set to default if the given string is null.
        /// </summary>
        /// <param name="value">String value to be parsed. </param>
        /// <returns>An array of parsed elements. Returns default value if the given string is null.</returns>
        /// <exception cref="FormatException">Thrown if the given string cannot be parsed.</exception>
        protected override TArrayType[] Parse(string value)
        {
            if(value == null) return default;
            var trimmedString = value.Trim();
            
            if (string.IsNullOrEmpty(trimmedString))
                throw new FormatException("Value cannot be empty.");
            
            if (!trimmedString.StartsWith('[') || !trimmedString.EndsWith(']'))
                throw new FormatException("Value must be enclosed in square brackets.");
            
            var elements = trimmedString.Substring(1, trimmedString.Length - 2).Split(',');
            TArrayType[] returnArray = new TArrayType[elements.Length];
            for(int i = 0; i < returnArray.Length; i++)
            {
                returnArray[i] = ParseItem(elements[i].Trim());
            }
            return returnArray;
        }
        
        /// <summary>
        /// Parses given string value to TArrayType.
        /// </summary>
        /// <param name="value">String value to be parsed.</param>
        /// <returns></returns>
        /// <exception cref="FormatException">Thrown if the given string cannot be parsed.</exception>
        protected abstract TArrayType ParseItem(string value);
    }
}