using System;
using System.Collections.Generic;


namespace TahaCore
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ApplicationRuntimeRegistryAttribute : Attribute
    {
        public IEnumerable<Type> RegisteredTypes => m_registeredTypes;
        private List<Type> m_registeredTypes = new();
        
        public LifetimeType LifetimeType => m_lifetimeType;
        private LifetimeType m_lifetimeType;

        public ApplicationRuntimeRegistryAttribute(LifetimeType lifetimeType, params Type [] interfaces)
        {
            m_lifetimeType = lifetimeType;
            m_registeredTypes.AddRange(interfaces);
        }
    }
}
