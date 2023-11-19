using TahaCore.Runtime.DI;
using UnityEngine;

namespace TahaCore.Tests.Runtime
{
    public abstract class RuntimeTestBoot
    {
        protected virtual string AdditionalConfig { get; } = null;
        protected TahaCoreApplicationRuntime Runtime { get; private set; }

        protected RuntimeTestBoot()
        {
            if (TahaCoreApplicationRuntime.Instance != null)
            {
                Object.DestroyImmediate(TahaCoreApplicationRuntime.Instance.gameObject);
            }

            if (AdditionalConfig != null)
            {
                TahaCoreApplicationRuntime.AdditionalConfigData = AdditionalConfig;
            }
            GameObject runtimeHost = new GameObject("TestRuntimeHost");
            Runtime = runtimeHost.AddComponent<TahaCoreApplicationRuntime>();
        }
    }
}
