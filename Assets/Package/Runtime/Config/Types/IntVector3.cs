using System;
using UnityEngine;

namespace TahaCore.Config.Types
{
    /// <summary>
    /// Container for Integer numbers. Use this to parse a config value to Vector3Int.<br/>
    /// Format is as follows: (1, 2, 3) <br/>
    /// Rules: <br/>
    /// - Entry must be enclosed in parenthesis.
    /// </summary>
    public class IntVector3 : VectorConfigType<Vector3Int, int>
    {
        public IntVector3(string value) : base(value, 3)
        {
        }

        public IntVector3() : base(3)
        {
        }

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

        protected override Vector3Int CreateVector(int[] data)
        {
            return new Vector3Int(data[0], data[1], data[2]);
        }
    }
}