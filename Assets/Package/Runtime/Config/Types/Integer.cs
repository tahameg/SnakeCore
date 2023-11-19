using System;

namespace TahaCore.Runtime.Config.Types
{
    /// <summary>
    /// Container for Integer numbers. Use this to parse a config value to Integer.
    /// </summary>
    public class Integer : ConfigType<int>
    {
        /// <inheritdoc cref="ConfigType{T}(string)"/>
        public Integer(string value) : base(value){}
        
        /// <inheritdoc cref="ConfigType{T}()"/>
        public Integer(){}
        
        /// <inheritdoc cref="ConfigType{T}.Parse(string)" />
        protected override int Parse(string value)
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