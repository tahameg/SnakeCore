using System;
using SnakeCore.Scene;
using UnityEngine;

namespace SnakeCore.ObjectPool
{
    /// <summary>
    /// Base class for poolable MonoBehaviours. <seealso cref="GameObjectPool"/>
    /// </summary>
    public class PoolableMonoBehaviour : SceneBehaviour, IPoolableObject
    {
        /// <summary>
        /// Returns the object to the pool.
        /// </summary>
        public bool? IsActive { get; private set; } = null;

        /// <summary>
        /// Pool that the object is managed by.
        /// </summary>
        internal IObjectPool Pool { private get; set; }
        
        internal void Activate()
        {
            if(IsActive != true)
            {
                IsActive = true;
                OnActivate();
            }
        }

        internal void Deactivate()
        {
            if (IsActive != false)
            {
                IsActive = false;
                OnDeactivate();
            }
        }
        
        public void ReturnToPool()
        {
            Pool?.ReturnObject(this);
        }
        
        /// <summary>
        /// Invoked right after the object is activated.
        /// </summary>
        protected virtual void OnActivate()
        {
            gameObject.SetActive(true);
        } 
        
        /// <summary>
        /// Invoked right after the object is deactivated.
        /// </summary>
        protected virtual void OnDeactivate()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDestroy()
        {
            Pool?.Remove(this);
        }
    }
}
