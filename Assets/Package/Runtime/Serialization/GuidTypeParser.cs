using System;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    [Preserve]
    internal class GuidTypeParser : ITypeParser
    {
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = true;

        /// <inheritdoc cref="ConfigType{T}(string)"/>
        internal GuidTypeParser()
        {
            TargetType = typeof(Guid);
        }

        public object Parse(string value)
        {
            if(value == null) return default;
            string trimmedValue = value.Trim();
            return System.Guid.Parse(trimmedValue);
        }
    }
}