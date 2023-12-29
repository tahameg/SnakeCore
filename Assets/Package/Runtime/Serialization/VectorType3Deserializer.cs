using System;
using TahaCore.Runtime.Config.Types;
using UnityEngine;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Container for Vector3 numbers. Use this to parse a config value to Vector3.
    /// The format is as follows: (1, 2, 3)
    /// <br/>
    /// Rules:
    /// <br/>
    /// - Entry must be enclosed in parenthesis.
    /// </summary>
    [Preserve]
    internal class VectorType3Deserializer : VectorTypeParser<Vector3, float>
    { 
        /// <inheritdoc cref="VectorConfigType{T, TData}(string, int)"/>
        internal VectorType3Deserializer() : base(3, new FloatTypeParser()){}
        
        protected override Vector3 CreateVector(float[] data)
        {
            return new UnityEngine.Vector3(data[0], data[1], data[2]);
        }
    }
}