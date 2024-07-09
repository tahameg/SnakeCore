using SnakeCore.DI;
using VContainer;

namespace SnakeCore.Scene
{
    /// <summary>
    /// Internally capable of sending and receiving new events to the event bus.
    /// </summary>
    public class SceneBehaviour : InjectableMonoBehaviour
    {
        [Inject] protected readonly ISceneEventHistory SceneEventHistory;
        [Inject] protected readonly ISceneEventProvider SceneEventPublisher;

    }

    
}
