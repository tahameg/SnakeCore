// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using UnityEngine;

namespace TahaCore.DI
{
    public abstract class InjectableMonoBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            TahaCoreApplicationRuntime.Instance.Container.Inject(this);
        }
    }
}