using System;

namespace TahaCore.Serialization
{
    public class StringTypeParser : ITypeParser
    {
        public Type TargetType { get; } = typeof(string);
        public bool CanBeArrayElement { get; } = true;
        public object Parse(string value)
        {
            return value.Trim();
        }
    }
}