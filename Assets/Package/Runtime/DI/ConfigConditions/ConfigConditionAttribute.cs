using System;
using TahaCore.Config;

namespace TahaCore.DI.ConfigConditions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public abstract class ConfigConditionAttribute : Attribute
    {
        public abstract bool Evaluate(IConfigManager manager);
    }
}