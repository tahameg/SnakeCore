using System;
using TahaCore.Serialization;
using UnityEngine.Scripting;

namespace TahaCore.Runtime.Config.Types
{
    /// <summary>
    /// Container for Integer numbers. Use this to parse a config value to Integer.
    /// </summary>
    [Preserve]
    internal class IntegerTypeParser : ITypeParser
    {
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = true;

        /// <inheritdoc cref="ConfigType{T}(string)"/>
        internal IntegerTypeParser()
        {
            TargetType = typeof(int);
        }

        /// <inheritdoc cref="ConfigType{T}.Parse(string)" />
        public object Parse(string value)
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