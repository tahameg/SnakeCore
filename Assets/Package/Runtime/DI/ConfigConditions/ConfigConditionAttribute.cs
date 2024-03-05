// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;
using TahaCore.Config;

namespace TahaCore.DI.ConfigConditions
{
    /// <summary>
    /// Base class that provide functionality of Config-based dependency injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public abstract class ConfigConditionAttribute : Attribute
    {
        public abstract bool Evaluate(IConfigValueProvider configValueProvider);
    }
}