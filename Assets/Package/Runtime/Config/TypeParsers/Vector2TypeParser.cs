// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using UnityEngine;
using UnityEngine.Scripting;

namespace SnakeCore.Config.TypeParsers
{
    /// <summary>
    /// Parser for parsing Vector2 from string. Use this to parser a string value to Vector2.<br/>
    /// The format is as follows: (1.0, 2.0) or (1, 2)<br/>
    /// Rules: <br/>
    /// - Entry must be enclosed in parenthesis.<br/>
    /// - No leading f or F for float values.<br/>
    /// </summary>
    [ConfigTypeParser]
    [Preserve]
    internal class Vector2TypeParser : VectorTypeParser<Vector2, float>
    {
        /// <summary>
        /// Creates a new instance of Vector2TypeParser.
        /// </summary>
        internal Vector2TypeParser() : base(2){}
        
        /// <summary>
        /// Creates a new Vector2 from the given data.
        /// </summary>
        /// <param name="data">Data to create the Vector2 from.</param>
        /// <returns>Vector2 created from the given data.</returns>
        protected override Vector2 CreateVector(float[] data)
        {
            return new Vector2(data[0], data[1]);
        }
    }
}