// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;

namespace TahaCore.Serialization
{
    /// <summary>
    /// ParserContextRegistryAttribute is used to mark a class to be registered to the AutoBoundTypeParserContext.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TypeParserContextRegistryAttribute : Attribute
    {
    }
}