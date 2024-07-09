using System;

namespace SnakeCore.Serialization
{
    /// <summary>
    /// Created to provide functionality of JsonProperty of NewtonSoft.Json.
    /// </summary>
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