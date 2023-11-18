using System;
using UnityEngine;

namespace TahaCore.Config.Types
{
    /// <summary>
    /// Container for Integer numbers. Use this to parse a config value to Vector2Int.
    /// The format is as follows: (1, 2)
    /// Rules:
    /// - Entry must be enclosed in parenthesis.
    /// </summary>
    public class IntVector2 : VectorConfigType<Vector2Int, int>
    {
        /// <inheritdoc cref="VectorConfigType{T, TData}(string, int)"/>
        public IntVector2(string value) : base(value, 2){}
        
        /// <inheritdoc cref="VectorConfigType{T, TData}()"/>
        public IntVector2() : base(2){}
        
        /// <inheritdoc cref="VectorConfigType{T, TData}.ParseData(string)" />
        protected override int ParseData(string value)
        {
            if(value == null) return default;
            try
            {
                return Convert.ToInt32(value.Trim());
            }
            catch (Exception)
            {
                throw new FormatException("Could not parse value to Integer");
            }
        }
        
        /// <inheritdoc cref="VectorConfigType{T, TData}.CreateVector" />
        protected override Vector2Int CreateVector(int[] data)
        {
            return new Vector2Int(data[0], data[1]);
        }
    }
}