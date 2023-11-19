using System;
using TahaCore.Runtime.Config;

namespace TahaCore.Runtime.DI.ConfigConditions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public abstract class ConfigConditionAttribute : Attribute
    {
        public abstract bool Evaluate(IConfigManager manager);
    }
}