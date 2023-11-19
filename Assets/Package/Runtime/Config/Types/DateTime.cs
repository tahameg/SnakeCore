using System;

namespace TahaCore.Runtime.Config.Types
{
    /// <summary>
    /// Container for DateTime. Use this to parse a config value to DateTime.<br/>
    /// </summary>
    public class DateTime : ConfigType<System.DateTime>
    {
        /// <inheritdoc cref="ConfigType{T}(string)"/>
        public DateTime(string value) : base(value){}
        
        /// <inheritdoc cref="ConfigType{T}()"/>
        public DateTime(){}

        protected override System.DateTime Parse(string value)
        {
            try
            {
                if (value == null) return default;
                string trimmedValue = value.Trim();
                return System.DateTime.Parse(trimmedValue);
            }
            catch (Exception)
            {
                throw new FormatException("Could not parse value to DateTime");

            }
        }
    }
}