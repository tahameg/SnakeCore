using SnakeCore.DI;
using UnityEngine;

namespace TahaCore.Tests.PlayMode
{
    public abstract class RuntimeTestBoot
    {
        protected abstract string AdditionalConfig { get; }
        protected SakeCoreApplicationRuntime Runtime { get; private set; }

        protected RuntimeTestBoot()
        {
            if (SakeCoreApplicationRuntime.Instance != null)
            {
                Object.DestroyImmediate(SakeCoreApplicationRuntime.Instance.gameObject);
            }
            Setup();
        }
        
        private void Setup()
        {
            SakeCoreApplicationRuntime.AdditionalConfigData = AdditionalConfig;
            GameObject runtimeHost = new GameObject("TestRuntimeHost");
            Runtime = runtimeHost.AddComponent<SakeCoreApplicationRuntime>();
        }
    }
}
