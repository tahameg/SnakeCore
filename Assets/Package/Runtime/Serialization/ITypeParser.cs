// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Interface for parsing data types from string. All types that implements this
    /// interface are cached by the <see cref="AutoBoundTypeParserContext"/> class in the constructor. So
    /// Make sure the implemented class is has the attribute <see cref="UnityEngine.Scripting.PreserveAttribute"/>
    /// so that it is not stripped by the IL2CPP compiler.
    /// </summary>
    public interface ITypeParser
    {
        /// <summary>
        /// Should return the type that this parser can parse.
        /// </summary>
        Type TargetType { get; }
        
        /// <summary>
        /// Can the type that can be parsed by this parser be an array element.
        /// For example, [1,2,3] can be parsed to an array of integers. But [[1,2,3],[4,5,6]] cannot be parsed
        /// to an array of arrays since the current ArrayTypeParser doesn't support recursive parsing.
        /// TODO: Add recursive parsing support ( Low priority )
        /// </summary>
        bool CanBeArrayElement { get; }
        
        /// <summary>
        /// Parse string value to generic type.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <returns>The parsed value. Returns default if the given value is null.</returns>
        /// <exception cref="FormatException">Thrown if the value cannot be parsed to type.</exception>
        object Parse(string value);
    }
}