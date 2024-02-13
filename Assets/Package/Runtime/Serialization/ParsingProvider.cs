// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System;
using TahaCore.DI;
using TahaCore.Serialization.TypeParsers;

namespace TahaCore.Serialization
{
    /// <summary>
    /// The implementation of IParsingProvider that handles deserializing of string values to a given type.
    /// This implementation ignores <see cref="ITypeParser"/>s that targets primitive types that
    /// are registered in the <see cref="ITypeParserLocator"/>.
    /// Instead, it internally calls <see cref="CommonTypeParser.TryParsePrimitive"/> to parse primitive types.
    /// </summary>
    internal class ParsingProvider : IParsingProvider
    {
        private readonly ITypeParserLocator m_typeParserLocator;
        internal ParsingProvider(ITypeParserLocator typeParserLocator)
        {
            m_typeParserLocator = typeParserLocator;
        }

        public object Parse(Type targetType, string value)
        {
            if(value == null) return null;
            if(CommonTypeParser.TryParsePrimitive(targetType, value, out var result)) return result;
            var deserializer = m_typeParserLocator.GetParserForType(targetType);
            if (deserializer != null) return deserializer.Parse(value);
            //else
            TahaCoreApplicationRuntime.LogWarning($"No parser found for type {targetType.Name}");
            return null;
        }
        
        public T Parse<T>(string value)
        {
            if(CommonTypeParser.TryParsePrimitive(typeof(T), value, out var result)) return (T)result;
            var deserializer = m_typeParserLocator.GetParserForType(typeof(T));
            if (deserializer != null) return (T)deserializer.Parse(value);
            //else
            TahaCoreApplicationRuntime.LogWarning($"No parser found for type {typeof(T).Name}");
            return default;
        }

        public bool CanParse(Type targetType)
        {
            return m_typeParserLocator.CanLocate(targetType) || CommonTypeParser.IsCommonType(targetType);
        }
    }
}