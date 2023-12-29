using System;

namespace TahaCore.Config
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ConfigPropertyAttribute : Attribute
    {
        public string PropertyName { get; private set; }
        public ConfigPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
