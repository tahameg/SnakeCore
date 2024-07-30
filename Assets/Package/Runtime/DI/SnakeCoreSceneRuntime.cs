using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SnakeCore.Reflection;
using UnityEngine;
using VContainer;

namespace SnakeCore.DI
{
    /// <summary>
    /// SnakeCoreSceneRuntime allow creating scene-based DI containers. This class is responsible for registering
    /// the types that are decorated with <see cref="SceneRuntimeRegistryAttribute"/>. Types that are registered with
    /// scene runtime registry attribute will still have access to the types that are registered with the
    /// <see cref="SnakeCoreApplicationRuntime"/> using <see cref="ApplicationRuntimeRegistryAttribute"/>
    /// For adding entry points to the scene runtime, use <see cref="EntryPointRegistryAttribute"/>.
    /// </summary>
    [DefaultExecutionOrder(-4990)]
    public class SnakeCoreSceneRuntime : Runtime
    {
        private static Dictionary<string, SnakeCoreSceneRuntime> s_sceneRuntimes = new();
        
        private static void RegisterSceneRuntime(string sceneName, SnakeCoreSceneRuntime runtime)
        {
            if(s_sceneRuntimes.ContainsKey(sceneName))
            {
                SnakeCoreApplicationRuntime.LogWarning($"Multiple SceneRuntimes are registered for the scene {sceneName}. " +
                                                       $"This is not allowed.");
                Destroy(runtime);
                return;
            }
            
            s_sceneRuntimes.Add(sceneName, runtime);
        }
        
        private static void UnregisterSceneRuntime(string sceneName)
        {
            if(s_sceneRuntimes.ContainsKey(sceneName))
            {
                s_sceneRuntimes.Remove(sceneName);
            }
        }
        
        protected override void Awake()
        {
            if (SnakeCoreApplicationRuntime.Instance == null)
            {
                throw new InvalidOperationException("SnakeCoreApplicationRuntime is not initialized.");
            }
            RegisterSceneRuntime(gameObject.scene.name, this);
            EnqueueParent(SnakeCoreApplicationRuntime.Instance);
            base.Awake();
        }
        
        protected override void OnDestroy()
        {
            UnregisterSceneRuntime(gameObject.scene.name);
            base.OnDestroy();
        }
        
        protected override IEnumerable<RegistrationInfo> GetRegistrationInfos()
        {
            var decoratedTypes 
                = TypeUtility.GetTypes(ShouldBeRegistered);
            
            var registrationInfos = new List<RegistrationInfo>();

            foreach (var type in decoratedTypes)
            {
                if(!DoesCoverConfigConditions(type)) continue;
                var attribute = type.GetCustomAttribute<SceneRuntimeRegistryAttribute>();
                var isEntryPoint = type.IsDefined(typeof(EntryPointRegistryAttribute), true);
                var info = new RegistrationInfo(type, Lifetime.Scoped, isEntryPoint, attribute.RegisteredTypes?.ToArray());
                registrationInfos.Add(info);
            }
            
            return registrationInfos;
        }
        
        private bool ShouldBeRegistered(Type type)
        {
            SceneRuntimeRegistryAttribute attribute = type.GetCustomAttribute<SceneRuntimeRegistryAttribute>(false);
            if(attribute == null) return false;
            string sceneName = gameObject.scene.name;
            return attribute.SceneName != null && string.Equals(attribute.SceneName, sceneName);
        }
    }
}