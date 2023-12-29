using System;
using TahaCore.Runtime.Config.Types;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Container for Double precision floating point numbers. Use this to parse a config value to Double.<br/>
    /// Data format is as follows: 1.0 or 1
    /// </summary>
    [Preserve]
    internal class DoubleTypeParser : ITypeParser
    {
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = true;

        internal DoubleTypeParser()
        {
            TargetType = typeof(double);
        }

        public object Parse(string value)
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