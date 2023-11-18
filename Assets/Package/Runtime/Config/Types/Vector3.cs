using System;

namespace TahaCore.Config.Types
{
    /// <summary>
    /// Container for Vector3 numbers. Use this to parse a config value to Vector3.
    /// The format is as follows: (1, 2, 3)
    /// <br/>
    /// Rules:
    /// <br/>
    /// - Entry must be enclosed in parenthesis.
    /// </summary>
    public class Vector3 : VectorConfigType<UnityEngine.Vector3, float>
    { 
        /// <inheritdoc cref="VectorConfigType{T, TData}(string, int)"/>
        public Vector3(string value) : base(value, 3){}
        
        /// <inheritdoc cref="VectorConfigType{T, TData}()"/>
        public Vector3() : base(3){}
        
        /// <inheritdoc cref="VectorConfigType{T, TData}.ParseData(string)" />
        protected override float ParseData(string value)
        {
            if(value == null) return default;
            try
            {
                return Convert.ToSingle(value.Trim());
            }
            catch (Exception)
            {
                throw new FormatException("Could not parse value to Single");
            }
        }

        protected override UnityEngine.Vector3 CreateVector(float[] data)
        {
            return new UnityEngine.Vector3(data[0], data[1], data[2]);
        }
    }
}