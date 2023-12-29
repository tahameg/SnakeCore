using System;
using TahaCore.Serialization;
using UnityEngine;
using UnityEngine.Scripting;

namespace TahaCore.Runtime.Config.Types
{
    /// <summary>
    /// Container for Integer numbers. Use this to parse a config value to Vector3Int.<br/>
    /// Format is as follows: (1, 2, 3) <br/>
    /// Rules: <br/>
    /// - Entry must be enclosed in parenthesis.
    /// </summary>
    [Preserve]
    internal class Vector3IntTypeParser : VectorTypeParser<Vector3Int, int>
    {
        internal Vector3IntTypeParser() : base(3, new IntegerTypeParser())
        {
        }

        protected override Vector3Int CreateVector(int[] data)
        {
            return new Vector3Int(data[0], data[1], data[2]);
        }
    }
}