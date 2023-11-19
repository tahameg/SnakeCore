using System;
using UnityEngine;

namespace TahaCore.DI
{
    public abstract class InjectableMonoBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            TahaCoreApplicationRuntime.Instance.Container.Inject(this);
        }
    }
}