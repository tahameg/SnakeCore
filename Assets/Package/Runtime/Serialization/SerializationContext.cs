using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TahaCore.DI;
using TahaCore.Reflection;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [Preserve]
    public class SerializationContext
    {
        private IReadOnlyDictionary<Type, SerializationInfo> SerializationInfos { get; set; }

        public SerializationContext()
        {
            var serializableTypes = TypeUtility.GetTypes(IsSerializableClass);
            PopulateSerializationInfos(serializableTypes);
        }
        
        public SerializationInfo GetSerializationInfo(Type type)
        {
            if (SerializationInfos.TryGetValue(type, out var serializationInfo))
            {
                return serializationInfo;
            }
            throw new ArgumentException($"Type {type} is not serializable.");
        }
        
        public PropertyInfo GetSerializablePropertyBySerializationName(Type type, string serializationName)
        {
            var serializationInfo = GetSerializationInfo(type);
            if (serializationInfo.SerializableProperties.TryGetValue(serializationName, out var property))
            {
                return property;
            }

            return null;
        }
        
        public KeyValuePair<string, PropertyInfo> GetSerializablePropertyByName(Type type, string propertyName)
        {
            var serializationInfo = GetSerializationInfo(type);
            var val = serializationInfo.SerializableProperties
                .FirstOrDefault(keyValue => keyValue.Value.Name == propertyName);

            return val;
        }
        
        public bool IsRegistered(Type type)
        {
            return SerializationInfos.ContainsKey(type);
        }

        private bool IsSerializableClass(Type type)
        {
            //Return false if type is null
            if (type == null) return false;
            
            // Can be serialized if it has SerializableClassAttribute and is not abstract or interface
            var attribute = type.GetCustomAttribute<SerializableClassAttribute>();
            return attribute != null && !type.IsAbstract && !type.IsInterface;
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
            SerializationInfos = serializationInfos;
        }

    }
}