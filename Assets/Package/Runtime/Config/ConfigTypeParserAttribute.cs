// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System;

namespace TahaCore.Config
{
    /// <summary>
    /// ConfigTypeParserAttribute is used to mark a class to be registered to the IniConfigTypeParserLocator.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ConfigTypeParserAttribute : Attribute
    {
    }
}