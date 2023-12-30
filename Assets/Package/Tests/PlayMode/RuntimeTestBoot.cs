using TahaCore.DI;
using UnityEngine;

namespace TahaCore.Tests.PlayMode
{
    public abstract class RuntimeTestBoot
    {
        protected abstract string AdditionalConfig { get; }
        protected TahaCoreApplicationRuntime Runtime { get; private set; }

        protected RuntimeTestBoot()
        {
            if (TahaCoreApplicationRuntime.Instance != null)
            {
                Object.DestroyImmediate(TahaCoreApplicationRuntime.Instance.gameObject);
            }
            Setup();
        }
        
        private void Setup()
        {
            TahaCoreApplicationRuntime.AdditionalConfigData = AdditionalConfig;
            GameObject runtimeHost = new GameObject("TestRuntimeHost");
            Runtime = runtimeHost.AddComponent<TahaCoreApplicationRuntime>();
        }
    }
}
