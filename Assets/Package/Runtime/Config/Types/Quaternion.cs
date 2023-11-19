using System;

namespace TahaCore.Runtime.Config.Types
{
    public class Quaternion : VectorConfigType<UnityEngine.Quaternion, float>
    {
        public Quaternion(string value) : base(value, 4)
        {
        }

        public Quaternion() : base(4)
        {
        }

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

        protected override UnityEngine.Quaternion CreateVector(float[] data)
        {
            return new UnityEngine.Quaternion(data[0], data[1], data[2], data[3]);
        }
    }
}