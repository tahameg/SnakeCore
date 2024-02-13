using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TahaCore.Serialization.TypeParsers;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Manages access to type parsers.
    /// </summary>
    public abstract class TypeParserLocator : ITypeParserLocator
    {
        private ICollection<ITypeParser> m_deserializers;

        private IDictionary<Type, ITypeParser> m_deserializersByType { get;}

        public TypeParserLocator()
        {
            m_deserializers = new List<ITypeParser>();
            m_deserializersByType = new Dictionary<Type, ITypeParser>();
        }
        /// <summary>
        /// Get parser for the given type.
        /// </summary>
        /// <param name="targetType">Type to get parser for.</param>
        /// <returns>Parser for the given type.</returns>
        /// <exception cref="ArgumentNullException">If the given type is null.</exception>
        public virtual ITypeParser GetParserForType(Type targetType)
        {
            if(targetType == null) throw new ArgumentNullException(nameof(targetType));
            m_deserializersByType.TryGetValue(targetType, out var found);
            return found;
        }
      
        /// <summary>
        /// Get parser of type T. If no parser is found, it tries to create a new parser of type T and register it.
        /// </summary>
        /// <typeparam name="T">Type of parser to get.</typeparam>
        /// <returns>Parser of type T. Null if no parser was found and cannot be created.</returns>
        public virtual ITypeParser GetParser<T>() where T : ITypeParser
        {
            var found = m_deserializers.FirstOrDefault(deserializer => deserializer.GetType() == typeof(T));
            if (found != null) return found;

            try
            {
                ITypeParser created = (ITypeParser)Activator.CreateInstance(typeof(T));
                if (created != null && TryRegisterParser(created))
                {
                    return created;
                }

            }
            catch (Exception)
            {
                // ignored
            }
            
            return null;
        }
        
        /// <summary>
        /// Returns true if a parser is registered for the given type.
        /// </summary>
        /// <param name="targetType">Type to check if a parser is registered for.</param>
        /// <returns>Ture if a parser is registered for the given type, false otherwise.</returns>
        public bool CanLocate(Type targetType)
        {
            return m_deserializersByType.ContainsKey(targetType);
        }

        /// <summary>
        /// Registers the given parser to the context.
        /// If a parser is already registered for the given type, it returns false.
        /// If overrideIfRegistered is true, it replaces the existing parser with the given one and returns true.
        /// </summary>
        /// <param name="parser">Parser to register.</param>
        /// <param name="overrideIfRegistered">If true, replaces the existing parser with the given one.</param>
        /// <returns>True if the parser is registered, false otherwise.</returns>
        protected bool TryRegisterParser(ITypeParser parser, bool overrideIfRegistered=false)
        {
            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser), "Parser cannot be null.");
            }
            if (m_deserializersByType.ContainsKey(parser.TargetType))
            {
                if (overrideIfRegistered)
                {
                    m_deserializers.Remove(m_deserializersByType[parser.TargetType]);
                    m_deserializersByType.Remove(parser.TargetType);
                }
                else
                {
                    return false;
                }
            }
            m_deserializers.Add(parser);
            m_deserializersByType.Add(parser.TargetType, parser);
            return true;
        }
    }
}