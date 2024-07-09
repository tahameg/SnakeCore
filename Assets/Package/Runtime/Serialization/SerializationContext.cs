using System;
using System.Collections.Generic;
using System.Reflection;
using SnakeCore.Reflection;
using UnityEngine.Scripting;

namespace SnakeCore.Serialization
{
    [Preserve]
    public class SerializationContext : ISerializationContext
    {
        private IReadOnlyDictionary<Type, SerializationInfo> m_serializationInfos;

        public SerializationContext()
        {
            var serializableTypes = TypeUtility.GetTypes(IsSerializableClass);
            PopulateSerializationInfos(serializableTypes);
        }
        
        public bool TryGetSerializationInfo(Type type, out SerializationInfo serializationInfo)
        {
            return m_serializationInfos.TryGetValue(type, out serializationInfo);
        }
        
        public bool IsSerializable(Type type)
        {
            return m_serializationInfos.ContainsKey(type);
        }
        
        private bool IsSerializableClass(Type type)
        {
            return type.IsDefined(typeof(SerializableTypeAttribute)) && !type.IsAbstract && !type.IsInterface;
        }
        
        private void PopulateSerializationInfos(IEnumerable<Type> serializableTypes)
        {
            var serializationInfos = new Dictionary<Type, SerializationInfo>();
            foreach (var serializableType in serializableTypes)
            {
                Dictionary<string, PropertyInfo> serializableProperties = new();
                var properties 
                    = serializableType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    var attribute = property.GetCustomAttribute<SerializablePropertyAttribute>();
                    if (attribute != null)
                    {
                        if (!serializableProperties.TryAdd(attribute.PropertyName, property))
                        {
                            throw new InvalidOperationException(
                                $"Type {serializableType}" +
                                $" has multiple properties with the same name {attribute.PropertyName}.");
                        }
                    }
                }
                serializationInfos.Add(serializableType, new SerializationInfo(serializableType, serializableProperties));
            }
            m_serializationInfos = serializationInfos;
        }

    }
}