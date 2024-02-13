// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;
using TahaCore.Serialization.TypeParsers;
using Unity.VisualScripting.YamlDotNet.Core;

namespace TahaCore.Config
{
    /// <summary>
    /// Used to specify a parser for a config property.
    /// Only valid for properties of classes that inherit from <see cref="ConfigSection"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ParseWithAttribute : Attribute
    {
        public Type ParserType { get; private set; }
        public ParseWithAttribute(Type parserType)
        {
            if (!typeof(ITypeParser).IsAssignableFrom(parserType))
            {
                throw new ArgumentException($"Parser type {parserType} is not assignable from {typeof(IParser)}");
            }
            ParserType = parserType;
        }
    }
}