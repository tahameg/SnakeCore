using System;
using TahaCore.DI;

namespace TahaCore.Serialization
{
    public class TypeParsingProvider : ITypeParsingProvider
    {
        private readonly TypeParserContext m_typeParserContext;
        
        internal TypeParsingProvider()
        {
            m_typeParserContext = new TypeParserContext();
        }
        
        public object Parse(Type targetType, string value)
        {
            var parser = m_typeParserContext.GetParserForType(targetType);
            if (parser != null) return parser.Parse(value);
            //else
            TahaCoreApplicationRuntime.LogWarning($"No parser found for type {targetType.Name}");
            return null;
        }

        public T Parse<T>(string value)
        {
            var parser = m_typeParserContext.GetParserForType(typeof(T));
            if (parser != null) return (T)parser.Parse(value);
            //else
            TahaCoreApplicationRuntime.LogWarning($"No parser found for type {typeof(T).Name}");
            return default;

        }
    }
}