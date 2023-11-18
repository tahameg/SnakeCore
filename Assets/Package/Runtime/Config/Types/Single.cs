using System;

namespace TahaCore.Config.Types
{
    /// <summary>
    /// Container for Single precision floating point numbers. Use this to parse a config value to Single.<br/>
    /// Data format is as follows: 1.0 or 1 <br/>
    /// Do not use f postfix.
    /// </summary>
    public class Single : ConfigType<float>
    {
        /// <inheritdoc cref="ConfigType{T}(string)"/>
        public Single(string value) : base(value){}
        
        /// <inheritdoc cref="ConfigType{T}()"/>
        public Single(){}
        
        /// <inheritdoc cref="ConfigType{T}.Parse(string)" />
        protected override float Parse(string value)
        {
            if(value == null) return default;
            try
            {
                return Convert.ToSingle(value.Trim());
            }
            catch (Exception)
            {
                throw new FormatException("Could not parse value to Single");
            }
        }
    }
}