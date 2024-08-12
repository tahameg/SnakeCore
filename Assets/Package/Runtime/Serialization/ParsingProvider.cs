// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System;
using SnakeCore.DI;
using SnakeCore.Serialization.JsonSerialization;
using Newtonsoft.Json;

namespace SnakeCore.Serialization
{
    /// <summary>
    /// The implementation of IParsingProvider that handles deserializing of string values to a given type.
    /// This implementation ignores <see cref="ITypeParser"/>s that targets primitive types that
    /// Instead, it internally calls <see cref="PrimitPrimitiveSerialization.TryDeserialize parse primitive types.
    /// </summary>
    internal class ParsingProvider : IParsingProvider
    {
        private IJsonSerializer m_serializer;
        public ParsingProvider(IJsonSerializer serializer)
        {
            m_serializer = serializer;
        }
        
        public object Parse(Type targetType, string value)
        {
            if(value == null) return null;
            if(PrimitiveSerialization.TryDeserialize(targetType, value, out var result)) return result;
            
            try
            {
                return m_serializer.Deserialize(value, targetType);
            }
            catch (JsonException)
            {
                //ignore
            }
            
            SnakeCoreApplicationRuntime.LogWarning($"Failed to parse {value} to {targetType.Name} using JsonDeserializer");
            return null;
        }
        
        public T Parse<T>(string value)
        {
            if(PrimitiveSerialization.TryDeserialize(typeof(T), value, out var result)) return (T)result;
            try
            {
                return (T)m_serializer.Deserialize(value, typeof(T));
            }
            catch (JsonException)
            {
                //ignore
            }
            
            SnakeCoreApplicationRuntime.LogWarning($"No parser found for type {typeof(T).Name}");
            return default;
        }

        public bool CanParse(Type targetType)
        {
            return PrimitiveSerialization.IsPrimitive(targetType);
        }
    }
}