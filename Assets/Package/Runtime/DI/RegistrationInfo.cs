// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace SnakeCore.DI
{
    /// <summary>
    /// Includes validated information about a registration.
    /// </summary>
    public class RegistrationInfo
    {
        private Type m_registrationTarget;
        private Lifetime m_lifetime;
        private bool m_selfRegistration;
        private bool m_isEntryPoint;
        private List<Type> m_entryPointInterfaces;

        /// <summary>
        /// Target type to register.
        /// </summary>
        public Type Target => m_registrationTarget;
        
        /// <summary>
        /// Is the target registered with itself.
        /// </summary>
        public bool SelfRegistration => m_selfRegistration;
        
        /// <summary>
        /// Lifetime of the registration.
        /// </summary>
        public Lifetime Lifetime => m_lifetime;
        
        
        public bool IsEntryPoint => m_isEntryPoint;
        
        private List<Type> m_registrationTypes;
        /// <summary>
        /// Interface type to register the target with.
        /// </summary>
        public IEnumerable<Type> RegistrationTypes => m_registrationTypes;

        public IReadOnlyList<Type> EntryPointInterfaces => m_entryPointInterfaces;
        
        
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
        internal RegistrationInfo(Type target, Lifetime lifetime, bool isEntryPoint=false, params Type[] registrationTypes)
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
            m_lifetime = lifetime;
            m_registrationTypes = ValidateRegistrationTypes(target, registrationTypes);

            m_isEntryPoint = isEntryPoint && TryGetEntryPointInterfaces(target, out m_entryPointInterfaces);
            
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
        
        private bool TryGetEntryPointInterfaces(Type type, out List<Type> entryPointInterfaces)
        {
            List<Type> types = new List<Type>()
            {
                typeof(IInitializable),
                typeof(IAsyncStartable),
                typeof(IPostInitializable),
                typeof(IStartable),
                typeof(IPostStartable),
                typeof(IFixedTickable),
                typeof(IPostFixedTickable),
                typeof(ITickable),
                typeof(IPostTickable),
                typeof(ILateTickable),
                typeof(IPostLateTickable)
            };
            
            entryPointInterfaces = new List<Type>();
            
            foreach (var entryPointType in types)
            {
                if (entryPointType.IsAssignableFrom(type))
                {
                    entryPointInterfaces.Add(entryPointType);
                }
            }
            

            if (entryPointInterfaces.Count == 0)
            {
                SnakeCoreApplicationRuntime
                    .LogError($"Type {type} is marked as entry point but does " +
                              $"not implement any of the entry point interfaces.");
                return false;
            }
            
            return true;
        }
    }
}