using UnityEngine;

namespace TahaCore.Runtime.DI
{
    public abstract class InjectableMonoBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            TahaCoreApplicationRuntime.Instance.Container.Inject(this);
        }
    }
}