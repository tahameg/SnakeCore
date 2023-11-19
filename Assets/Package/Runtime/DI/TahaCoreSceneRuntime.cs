using UnityEngine;
using VContainer.Unity;

namespace TahaCore.DI
{
    [DefaultExecutionOrder(-4999)]
    public class TahaCoreSceneRuntime : LifetimeScope
    {
        protected override void Awake()
        {
            EnqueueParent(TahaCoreApplicationRuntime.Instance);
            base.Awake();
        }
        
    }
}