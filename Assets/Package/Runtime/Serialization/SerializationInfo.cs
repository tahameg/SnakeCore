using System;
using System.Collections.Generic;
using System.Reflection;

namespace TahaCore.Serialization
{
    public class SerializationInfo
    {
        public Type Type { get; private set; }
        public IReadOnlyDictionary<string, PropertyInfo> SerializableProperties { get; private set; }
        
        public SerializationInfo(Type type, IReadOnlyDictionary<string, PropertyInfo> serializableProperties)
        {
            Type = type;
            SerializableProperties = serializableProperties;
        }
    }
}