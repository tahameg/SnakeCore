using TahaCore.Runtime.Config.Types;
using UnityEngine;
using UnityEngine.Scripting;

namespace TahaCore.Serialization
{
    [Preserve]
    internal class QuaternionTypeParser : VectorTypeParser<Quaternion, float>
    {
        internal QuaternionTypeParser(string value) : base(4, new FloatTypeParser())
        {
        }

        protected override Quaternion CreateVector(float[] data)
        {
            return new Quaternion(data[0], data[1], data[2], data[3]);
        }
    }
}