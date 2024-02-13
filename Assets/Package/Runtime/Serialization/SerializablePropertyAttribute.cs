using System;

namespace TahaCore.Serialization
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SerializablePropertyAttribute : Attribute
    {
        public string PropertyName { get; }
        
        public SerializablePropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}