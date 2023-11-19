using System;

namespace TahaCore.Runtime.Config.Types
{
    /// <summary>
    /// Container for Double precision floating point numbers. Use this to parse a config value to Double.<br/>
    /// Data format is as follows: 1.0 or 1
    /// </summary>
    public class Double : ConfigType<double>
    {
        /// <inheritdoc cref="ConfigType{T}(string)"/>
        public Double(string value) : base(value){}
        
        
        /// <inheritdoc cref="ConfigType{T}()"/>
        public Double(){}
        
        /// <inheritdoc cref="ConfigType{T}.Parse(string)" />
        protected override double Parse(string value)
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