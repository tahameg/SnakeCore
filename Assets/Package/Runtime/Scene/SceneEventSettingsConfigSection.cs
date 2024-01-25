using TahaCore.Config;
using TahaCore.DI;
using UnityEngine.Scripting;

namespace TahaCore.Scene
{
    [ConfigSection("SceneEventSettings")]
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [Preserve]
    public class SceneEventSettingsConfigSection : ConfigSection
    {
        [ConfigProperty("MAX_HISTORY_SIZE")]
        public int MaxHistorySize { get; private set; }
    }
}