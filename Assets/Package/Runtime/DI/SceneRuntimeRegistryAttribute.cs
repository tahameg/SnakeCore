// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System;
using System.Collections.Generic;

namespace SnakeCore.DI
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SceneRuntimeRegistryAttribute : Attribute
    {
        public string SceneName { get; private set; }
        
        /// <summary>
        /// Registered types with this attribute.
        /// </summary>
        public IEnumerable<Type> RegisteredTypes => m_registeredTypes;
        private readonly List<Type> m_registeredTypes = new();
        
        public SceneRuntimeRegistryAttribute(string sceneName, params Type [] interfaces)
        {
            SceneName = sceneName;
            m_registeredTypes.AddRange(interfaces);
        }
    }
}