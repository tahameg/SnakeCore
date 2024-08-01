// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using UnityEngine;

namespace SnakeCore.DI
{
    public abstract class InjectableMonoBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            var sceneRuntime = SnakeCoreSceneRuntime.GetSceneRuntime(gameObject.scene.name);
            if(sceneRuntime != null)
            {
                sceneRuntime.Container.Inject(this);
            }
            else
            {
                SnakeCoreApplicationRuntime.Instance.Container.Inject(this);
            }
            
        }
    }
}