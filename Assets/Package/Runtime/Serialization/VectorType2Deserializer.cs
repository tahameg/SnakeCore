using TahaCore.Runtime.Config.Types;
using UnityEngine;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Container for Vector2. Use this to parse a config value to Vector2.<br/>
    /// Config format is as follows: (1.0, 2.0) or (1, 2)<br/>
    /// Rules: <br/>
    /// - Entry must be enclosed in parenthesis.<br/>
    /// </summary>
    [Preserve]
    internal class VectorType2Deserializer : VectorTypeParser<Vector2, float>
    {
        public VectorType2Deserializer() : base(2, new FloatTypeParser()){}
        
        protected override Vector2 CreateVector(float[] data)
        {
            return new Vector2(data[0], data[1]);
        }
    }
}