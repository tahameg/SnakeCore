using System;
using TahaCore.Runtime.DI;
using VContainer;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Base class for vector types. Inherit this to create a new vector type.
    /// - Format check is handled automatically.
    /// - Only override the ParseData method to parse the data type. And CreateVector to create the vector.
    /// </summary>
    /// <typeparam name="TVectorType"></typeparam>
    /// <typeparam name="TVectorDataType"></typeparam>
    public abstract class VectorTypeParser<TVectorType, TVectorDataType> : ITypeParser
    {
        
        public Type TargetType { get; }
        public bool CanBeArrayElement { get; } = false;

        /// <summary>
        /// dimension of the vector.
        /// </summary>
        private readonly int m_dimension;
        private ITypeParser m_elementTypeParser;

        protected VectorTypeParser(int dimension, ITypeParser elementTypeParser)
        {
            m_dimension = dimension;
            m_elementTypeParser = elementTypeParser;
            TargetType = typeof(TVectorType);
        }

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
                vector[i] = (TVectorDataType)m_elementTypeParser.Parse(split[i].Trim());
            }

            return CreateVector(vector);
        }
        
        protected abstract TVectorType CreateVector(TVectorDataType[] data);
    }
}