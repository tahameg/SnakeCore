using System;

namespace TahaCore.Runtime.Config.Types
{
    /// <summary>
    /// Container for Double Array. Use this to parse a config value to Double Array.<br/>
    /// The array format is as follows: [1.0, 2.0, 3.0] or [1, 2, 3]<br/>
    /// Values without decimal point are accepted.
    /// </summary>
    public class DoubleArray : ArrayConfigType<double>
    {
        /// <inheritdoc cref="ArrayConfigType{T}(string)"/>
        public DoubleArray(string value) : base(value){}
        
        /// <inheritdoc cref="ArrayConfigType{T}()"/>
        public DoubleArray(){}
        
        /// <inheritdoc cref="ArrayConfigType{T}.ParseItem(string)" />
        protected override double ParseItem(string value)
        {
            if(value == null) return default;
            try
            {
                return Convert.ToDouble(value.Trim());
            }
            catch (Exception)
            {
                throw new FormatException("Could not parse value to Double");
            }
        }
    }
}