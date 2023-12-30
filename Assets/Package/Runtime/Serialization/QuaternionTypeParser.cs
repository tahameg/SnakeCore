// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using UnityEngine;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Parser for parsing Quaternion from string. Use this to parse a string value to Quaternion.
    /// </summary>
    [TypeParserContextRegistry]
    internal class QuaternionTypeParser : VectorTypeParser<Quaternion, float>
    {
        /// <summary>
        /// Creates a new instance of QuaternionTypeParser.
        /// </summary>
        internal QuaternionTypeParser() : base(4, new FloatTypeParser())
        {
        }
        
        /// <summary>
        /// Creates a new Quaternion from the given data.
        /// </summary>
        /// <param name="data">Data to create the Quaternion from.</param>
        /// <returns>Quaternion created from the given data.</returns>
        protected override Quaternion CreateVector(float[] data)
        {
            return new Quaternion(data[0], data[1], data[2], data[3]);
        }
    }
}