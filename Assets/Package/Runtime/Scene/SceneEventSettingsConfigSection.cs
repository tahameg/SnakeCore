using SnakeCore.Config;
using SnakeCore.DI;
using UnityEngine.Scripting;

namespace SnakeCore.Scene
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