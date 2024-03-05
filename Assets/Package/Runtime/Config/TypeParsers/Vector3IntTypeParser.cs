// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using UnityEngine;
using UnityEngine.Scripting;

namespace TahaCore.Config.TypeParsers
{
    /// <summary>
    /// Parser for parsing Vector3Int from string. Use this to parser a string value to Vector3Int.<br/>
    /// The Format is as follows: (1, 2, 3) <br/>
    /// Rules: <br/>
    /// - Entry must be enclosed in parenthesis.
    /// </summary>
    [ConfigTypeParser]
    [Preserve]
    internal class Vector3IntTypeParser : VectorTypeParser<Vector3Int, int>
    {
        /// <summary>
        /// Creates a new instance of Vector3IntTypeParser.
        /// </summary>
        internal Vector3IntTypeParser() : base(3)
        {
        }
        
        /// <summary>
        /// Creates a new Vector3Int from the given data.
        /// </summary>
        /// <param name="data">Data to create the Vector3Int from.</param>
        /// <returns>Vector3Int created from the given data.</returns>
        protected override Vector3Int CreateVector(int[] data)
        {
            return new Vector3Int(data[0], data[1], data[2]);
        }
    }
}