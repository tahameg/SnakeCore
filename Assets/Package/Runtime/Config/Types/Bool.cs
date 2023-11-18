using System;

namespace TahaCore.Config.Types
{
    /// <summary>
    /// Container for bool config value. Use this to parse config value to bool.
    /// Format is as follows:
    /// - case insensitive
    /// - use "true" or "1" for true
    /// - use "false" or "0" for false
    /// </summary>
    public class Bool : ConfigType<bool>
    {
        /// <inheritdoc cref="ConfigType{T}(string)"/>
        public Bool(string value) : base(value){}
        
        /// <inheritdoc cref="ConfigType{T}()"/>
        public Bool(){}
        
        /// <summary>
        /// Parses the given string value to bool. If the value is null, false is returned.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <returns>True if value is "1" or "True". False if value is "0" or "False".</returns>
        /// <exception cref="FormatException">Thrown the value doesn't have one of the given values.</exception>
        protected override bool Parse(string value)
        {
            if (value == null) return false;
            string fixedValue = value.Trim().ToLower();
            if (fixedValue == "true") return true;
            if (fixedValue == "false") return false;
            if (fixedValue == "1") return true;
            if (fixedValue == "0") return false;
            throw new FormatException($"Invalid bool value: {value}");
        }
    }
}