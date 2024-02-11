// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System;
using TahaCore.Serialization;
using TahaCore.Serialization.TypeParsers;

namespace TahaCore.Config.TypeDeserializers
{
    /// <summary>
    /// Base class for parsers for vector types. Inherit this to create a new parser for a vector type..
    /// - Format check is handled automatically.
    /// - Only override the CreateVector method to create the vector instance using the given data.
    /// </summary>
    /// <typeparam name="TVectorType">Type of the vector.</typeparam>
    /// <typeparam name="TVectorDataType">Type of the data that vector contains.</typeparam>
    public abstract class VectorTypeParser<TVectorType, TVectorDataType> : ITypeParser
    {
        
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = false;

        /// <summary>
        /// dimension of the vector.
        /// </summary>
        private readonly int m_dimension;

        /// <summary>
        /// Inherited by the child class.
        /// </summary>
        /// <param name="dimension">Dimension of the vector.</param>
        /// <param name="elementTypeDeserializer"></param>
        protected VectorTypeParser(int dimension)
        {
            m_dimension = dimension;
            TargetType = typeof(TVectorType);
        }
        
        /// <summary>
        /// Parses the given string value to vector. If the value is null, default value is returned.
        /// </summary>
        /// <param name="value">String value to parser.</param>
        /// <returns>Vector value of the given string.</returns>
        /// <exception cref="FormatException">If the given string is not a valid vector.</exception>
        public object Parse(string value)
        {
            if(value == null) return default;
            string trimmed = value.Trim();
            
            if (string.IsNullOrEmpty(trimmed))
                throw new FormatException("Value cannot be empty.");
            
            if(!trimmed.StartsWith('(') || !trimmed.EndsWith(')'))
                throw new FormatException("Value must be enclosed in parentheses.");
            
            var split = value.Substring(1, value.Length-2).Split(',');
            if (split.Length != m_dimension)
            {
                throw new FormatException($"Invalid vector value: {value}. Size must be {m_dimension}.");
            }
            
            var vector = new TVectorDataType[m_dimension];
            for (var i = 0; i < m_dimension; i++)
            {
                if (!CommonTypeParser.TryParsePrimitive(typeof(TVectorDataType), split[i].Trim(), out var parsed))
                {
                    throw new FormatException($"Invalid value to parser {nameof(TVectorType)}: {split[i]}.");
                }
                vector[i] = (TVectorDataType)parsed;
            }

            return CreateVector(vector);
        }
        
        /// <summary>
        /// Override this to create and return the vector.
        /// </summary>
        /// <param name="data">Data for the vector.</param> 
        /// <returns>Vector created from the given data.</returns>
        protected abstract TVectorType CreateVector(TVectorDataType[] data);
    }
}