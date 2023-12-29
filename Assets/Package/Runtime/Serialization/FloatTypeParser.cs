using System;
using TahaCore.Serialization;
using UnityEngine.Scripting;

namespace TahaCore.Runtime.Config.Types
{
    /// <summary>
    /// Container for Single precision floating point numbers. Use this to parse a config value to Single.<br/>
    /// Data format is as follows: 1.0 or 1 <br/>
    /// Do not use f postfix.
    /// </summary>
    [Preserve]
    internal class FloatTypeParser : ITypeParser
    {

        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = true;

        internal FloatTypeParser()
        {
            TargetType = typeof(float);
        }
        
        public object Parse(string value)
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