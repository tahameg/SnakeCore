// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using UnityEngine;
using UnityEngine.Scripting;

namespace TahaCore.Config.TypeParsers
{
    /// <summary>
    /// Parser for parsing Vector2Int from string. Use this to parser a string value to Vector2Int.<br/>
    /// The format is as follows: (1, 2)<br/>
    /// Rules:
    /// - Entry must be enclosed in parenthesis.
    /// </summary>
    [ConfigTypeParser]
    [Preserve]
    internal class Vector2IntTypeParser : VectorTypeParser<Vector2Int, int>
    {
        /// <summary>
        /// Creates a new instance of Vector2IntTypeParser.
        /// </summary>
        internal Vector2IntTypeParser() : base(2){}
        
        /// <summary>
        /// Creates a new Vector2Int from the given data.
        /// </summary>
        /// <param name="data">Data to create the Vector2Int from.</param>
        /// <returns>Vector2Int created from the given data.</returns>
        protected override Vector2Int CreateVector(int[] data)
        {
            return new Vector2Int(data[0], data[1]);
        }
    }
}