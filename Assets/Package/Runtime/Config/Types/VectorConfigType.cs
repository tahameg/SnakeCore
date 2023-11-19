using System;

namespace TahaCore.Runtime.Config.Types
{
    /// <summary>
    /// Base class for vector types. Inherit this to create a new vector type.
    /// - Format check is handled automatically.
    /// - Only override the ParseData method to parse the data type. And CreateVector to create the vector.
    /// </summary>
    /// <typeparam name="TVectorType"></typeparam>
    /// <typeparam name="TVectorDataType"></typeparam>
    public abstract class VectorConfigType<TVectorType, TVectorDataType> : ConfigType<TVectorType>
    {
        /// <summary>
        /// dimension of the vector.
        /// </summary>
        private readonly int m_dimension;
        
        /// <summary>
        /// Create a new instance of VectorConfigType with given value parsed with the given dimension.
        /// Default value of TVectorType is used if value is null.
        /// </summary>
        /// <param name="value">Value to parse to vector.</param>
        /// <param name="dimension">Dimension of vector.</param>
        /// <exception cref="FormatException">Thrown if given value cannot be parsed to the Vector.
        /// Also Thrown if the given value includes more field the the dimension.</exception>
        protected VectorConfigType(string value, int dimension)
        {
            m_dimension = dimension;
            SetValue(value);
        }

        /// <summary>
        /// Create a new instance of VectorConfigType with default value of TVectorType with the given dimension.
        /// </summary>
        /// <param name="dimension">Dimension of vector.</param>
        protected VectorConfigType(int dimension)
        {
            m_dimension = dimension;
        }

        
        /// <inheritdoc cref="ConfigType{T}.Parse(string)" />
        protected override TVectorType Parse(string value)
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
                vector[i] = ParseData(split[i]);
            }

            return CreateVector(vector);
        }
        
        /// <summary>
        /// Parse the data type from string.
        /// </summary>
        /// <param name="value">String Value to parse.</param>
        /// <returns>Parsed value. Returns default if the value is null.</returns>
        protected abstract TVectorDataType ParseData(string value);
        
        /// <summary>
        /// Create a new instance of TVectorType with the given data.
        /// </summary>
        /// <param name="data">Data in array format.</param>
        /// <returns>A new value of TVectorType that is created with the given data.</returns>
        protected abstract TVectorType CreateVector(TVectorDataType[] data);
    }
}