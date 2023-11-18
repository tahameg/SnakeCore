using System;
using System.Collections.Generic;
using TahaCore.DI.ConfigConditions;

namespace TahaCore.DI
{
    /// <summary>
    /// Includes validated information about a registration.
    /// </summary>
    internal class RegistrationInfo
    {
        private Type m_registrationTarget;
        private LifetimeType m_lifetimeType;
        private bool m_selfRegistration;

        /// <summary>
        /// Target type to register.
        /// </summary>
        internal Type Target => m_registrationTarget;
        
        /// <summary>
        /// Is the target registered with itself.
        /// </summary>
        internal bool SelfRegistration => m_selfRegistration;
        
        /// <summary>
        /// Lifetime of the registration.
        /// </summary>
        internal LifetimeType LifetimeType => m_lifetimeType;
        
        private List<Type> m_registrationTypes;
        /// <summary>
        /// Interface type to register the target with.
        /// </summary>
        public IEnumerable<Type> RegistrationTypes => m_registrationTypes;
        
        
        /// <summary>
        /// Registration info holds validated information about a registration. If no registration types are provided,
        /// the target type will be registered with itself.
        /// </summary>
        /// <param name="target">Target type to register. This MUST be a class type.</param>
        /// <param name="lifetimeType">Life time that this type will be registered with.</param>
        /// <param name="registrationTypes">Interface types to register the target type. Target MUST be
        /// implementing those interfaces.</param>
        /// <exception cref="ArgumentNullException">If target is null. If any of the
        /// registrationTypes are null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If target type is not class type.
        /// If any of registrationTypes are not interface type.
        /// </exception>
        internal RegistrationInfo(Type target, LifetimeType lifetimeType, params Type[] registrationTypes)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target), "Target cannot be null.");
            }

            if (!target.IsClass)
            {
                throw new ArgumentException("Target must be a class type.", nameof(target));
            }
            
            m_registrationTarget = target;
            m_lifetimeType = lifetimeType;
            m_registrationTypes = ValidateRegistrationTypes(target, registrationTypes);
            
            if (m_registrationTypes == null || m_registrationTypes.Count == 0)
            {
                m_selfRegistration = true;
            }
            
        }
        
        private List<Type> ValidateRegistrationTypes(Type target, Type[] registrationTypes)
        {
            if (registrationTypes == null) return null;
            List<Type> typesToRegister = new();
            foreach (var type in registrationTypes)
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(Target), "Registration types cannot be null.");
                }

                if (!type.IsInterface)
                {
                    throw new ArgumentException("Registration types must be interface types.", nameof(Target));
                }

                if (!type.IsAssignableFrom(Target))
                {
                    throw new ArgumentException($"Registration target {target} does not implement {type}.", nameof(Target));
                }
                typesToRegister.Add(type);
            }

            return typesToRegister;
        }
    }
}