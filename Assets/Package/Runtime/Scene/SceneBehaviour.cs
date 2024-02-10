using TahaCore.DI;
using UnityEngine;
using VContainer;

namespace TahaCore.Scene
{
    public class SceneBehaviour : InjectableMonoBehaviour
    {
        [Inject] protected readonly ISceneEventHistory SceneEventHistory;
        [Inject] protected readonly ISceneEventProvider SceneEventPublisher;

    }

    
}
