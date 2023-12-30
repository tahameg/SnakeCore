// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;
using TahaCore.DI;

namespace TahaCore.Serialization
{
    /// <summary>
    /// The implementation of ITypeParsingProvider that handles parsing operations using the registered TypeParsers
    /// in TypeParserContext<see cref="AutoBoundTypeParserContext"/>.
    /// </summary>
    public class TypeParsingProvider : ITypeParsingProvider
    {
        private readonly ITypeParserContext m_typeParserContext;
        
        internal TypeParsingProvider(ITypeParserContext typeParserContext)
        {
            m_typeParserContext = typeParserContext;
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