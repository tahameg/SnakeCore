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
            SnakeCoreApplicationRuntime.Instance.Container.Inject(this);
        }
    }
}