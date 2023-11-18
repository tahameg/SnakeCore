using System;

namespace TahaCore.Config.Types
{
    /// <summary>
    /// Container for Integer numbers. Use this to parse a config value to Integer.
    /// Accepts array format: [1, 2, 3]
    /// </summary>
    public class IntegerArray : ArrayConfigType<int>
    {
        /// <inheritdoc cref="ArrayConfigType{T}(string)"/>
        public IntegerArray(string value) : base(value){}
        
        /// <inheritdoc cref="ArrayConfigType{T}()"/>
        public IntegerArray(){}
        
        /// <inheritdoc cref="ArrayConfigType{T}.ParseItem(string)" />
        protected override int ParseItem(string value)
        {
            if(value == null) return default;
            try
            {
                return Convert.ToInt32(value.Trim());
            }
            catch (Exception)
            {
                throw new FormatException("Could not parse value to Integer");
            }
        }
    }
}