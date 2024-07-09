using SnakeCore.DI;
using UnityEngine;

namespace TahaCore.Tests.PlayMode
{
    public abstract class RuntimeTestBoot
    {
        protected abstract string AdditionalConfig { get; }
        protected SnakeCoreApplicationRuntime Runtime { get; private set; }

        protected RuntimeTestBoot()
        {
            if (SnakeCoreApplicationRuntime.Instance != null)
            {
                Object.DestroyImmediate(SnakeCoreApplicationRuntime.Instance.gameObject);
            }
            Setup();
        }
        
        private void Setup()
        {
            SnakeCoreApplicationRuntime.AdditionalConfigData = AdditionalConfig;
            GameObject runtimeHost = new GameObject("TestRuntimeHost");
            Runtime = runtimeHost.AddComponent<SnakeCoreApplicationRuntime>();
        }
    }
}
