// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using UnityEngine;
using UnityEngine.Scripting;

namespace TahaCore.Config.TypeParsers
{
    /// <summary>
    /// Parser for parsing Vector3 from string. Use this to parser a string value to Vector3.<br/>
    /// The format is as follows: (1, 2, 3)
    /// <br/>
    /// Rules:<br/>
    /// - Entry must be enclosed in parenthesis.
    /// </summary>
    [ConfigTypeParser]
    [Preserve]
    internal class Vector3TypeParser : VectorTypeParser<Vector3, float>
    { 
        /// <summary>
        /// Creates a new instance of Vector3TypeParser.
        /// </summary>
        internal Vector3TypeParser() : base(3){}
        
        /// <summary>
        /// Creates a new Vector3 from the given data.
        /// </summary>
        /// <param name="data">Data to create the Vector3 from.</param>
        /// <returns>Vector3 created from the given data.</returns>
        protected override Vector3 CreateVector(float[] data)
        {
            return new UnityEngine.Vector3(data[0], data[1], data[2]);
        }
    }
}