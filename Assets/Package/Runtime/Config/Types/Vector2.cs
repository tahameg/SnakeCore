using System;

namespace TahaCore.Config.Types
{
    /// <summary>
    /// Container for Vector2. Use this to parse a config value to Vector2.<br/>
    /// Config format is as follows: (1.0, 2.0) or (1, 2)<br/>
    /// Rules: <br/>
    /// - Entry must be enclosed in parenthesis.<br/>
    /// </summary>
    public class Vector2 : VectorConfigType<UnityEngine.Vector2, float>
    {
        /// <inheritdoc cref="VectorConfigType{T, TData}(string, int)"/>
        public Vector2(string value) : base(value, 2){}
        
        /// <inheritdoc cref="VectorConfigType{T, TData}()"/>
        public Vector2() : base(2){}

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

        protected override UnityEngine.Vector2 CreateVector(float[] data)
        {
            return new UnityEngine.Vector2(data[0], data[1]);
        }
    }
}