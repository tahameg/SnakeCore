using System;
using System.Collections.Generic;

namespace TahaCore.Runtime.DI
{
    /// <summary>
    /// Attribute is used to register types in the application runtime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ApplicationRuntimeRegistryAttribute : Attribute
    {
        /// <summary>
        /// Registered types with this attribute.
        /// </summary>
        public IEnumerable<Type> RegisteredTypes => m_registeredTypes;
        private List<Type> m_registeredTypes = new();
        
        public LifetimeType LifetimeType => m_lifetimeType;
        private LifetimeType m_lifetimeType;

        /// <summary>
        /// Attribute is used to register types in the application runtime. lifetimeType is specifies
        /// either the same instance will be injected or a new instance will be injected.
        /// </summary>
        /// <param name="lifetimeType">Lifetime scope of the type.</param>
        /// <param name="interfaces">Interface types to register this class with.</param>
        public ApplicationRuntimeRegistryAttribute(LifetimeType lifetimeType, params Type [] interfaces)
        {
            m_lifetimeType = lifetimeType;
            m_registeredTypes.AddRange(interfaces);
        }
    }
}
