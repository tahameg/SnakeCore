using System;
using TahaCore.Serialization;
using UnityEngine;
using UnityEngine.Scripting;

namespace TahaCore.Runtime.Config.Types
{
    /// <summary>
    /// Container for Integer numbers. Use this to parse a config value to Vector2Int.
    /// The format is as follows: (1, 2)
    /// Rules:
    /// - Entry must be enclosed in parenthesis.
    /// </summary>
    [Preserve]
    internal class Vector2IntTypeParser : VectorTypeParser<Vector2Int, int>
    {
        internal Vector2IntTypeParser() : base(2, new IntegerTypeParser()){}
        
        protected override Vector2Int CreateVector(int[] data)
        {
            return new Vector2Int(data[0], data[1]);
        }
    }
}