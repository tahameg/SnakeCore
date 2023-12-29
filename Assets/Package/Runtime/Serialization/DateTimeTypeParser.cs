using System;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Container for DateTime. Use this to parse a config value to DateTime.<br/>
    /// </summary>
    [Preserve]
    internal class DateTimeTypeParser : ITypeParser
    {
        
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = true;

        internal DateTimeTypeParser()
        {
            TargetType = typeof(DateTime);
        }

        public object Parse(string value)
        {
            try
            {
                if (value == null) return default;
                string trimmedValue = value.Trim();
                return DateTime.Parse(trimmedValue);
            }
            catch (Exception)
            {
                throw new FormatException("Could not parse value to DateTime");
            }
        }
    }
}