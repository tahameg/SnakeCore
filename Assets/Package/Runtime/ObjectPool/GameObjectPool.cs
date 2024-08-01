using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SnakeCore.ObjectPool
{
    /// <summary>
    /// Object pool that creates and stores GameObjects.
    /// </summary>
    /// <typeparam name="T">Type of PoolableMonoBehaviour</typeparam>
    public class GameObjectPool<T> : IObjectPool, IObjectPoolGet<T> where T : PoolableMonoBehaviour
    {
        /// <summary>
        /// Number of GameObjects to be created when the pool is empty.
        /// </summary>
        public int BatchSize
        {
            get => m_batchSize;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(m_batchSize), "Batch size must be greater than zero");
                }
                m_batchSize = value;
            }
        }
        
        /// <summary>
        /// Number of objects that are created by the pool.
        /// </summary>
        public int ObjectCount => m_objects.Count;
        
        /// <summary>
        /// Container transform for the GameObjects. The container might be recreated if it is destroyed.
        /// </summary>
        public Transform Container => m_container;
        
        
        private readonly HashSet<T> m_objects = new();
        private readonly Func<GameObject> m_gameObjectFactory;
        private readonly Action<T> m_behaviourInitializer;
        private readonly bool m_dontDestroyOnLoad;
        private readonly object m_lockObject = new object();
        
        private Transform m_container;
        
        private int m_batchSize;
        private int m_objectInPoolCount;
        
        
        /// <summary>
        /// Creates a new GameObjectPool.
        /// </summary>
        /// <param name="gameObjectFactory">
        ///     Factory method for creating GameObjects when needed.
        /// </param>
        /// <param name="behaviourInitializer">
        ///     This method is a callback that is for customizing the
        ///     PoolableMonoBehaviour after the creation.</param>
        /// <param name="initialSize">
        ///     Initial number of GameObjects to be created.If zero,
        ///     no GameObject will be created at the start.
        /// </param>
        /// <param name="batchSize">
        ///     Number of GameObjects to be created when the pool is empty.
        /// </param>
        /// <param name="container">
        ///     Parent transform for the GameObjects. If null, a new GameObject will be created.
        /// </param>
        /// <param name="dontDestroyOnLoad">
        ///     If true, the container will not be destroyed when a new scene is loaded.
        /// </param>
        public GameObjectPool(
            Func<GameObject> gameObjectFactory = null, 
            Action<T> behaviourInitializer = null, 
            int initialSize = 10,
            int batchSize = 10, 
            Transform container = null, 
            bool dontDestroyOnLoad = false)
        {
            m_container = container ?? new GameObject("GameObjectPool").transform;
            m_dontDestroyOnLoad = dontDestroyOnLoad;
            
            if(dontDestroyOnLoad)
            {
                Object.DontDestroyOnLoad(m_container.gameObject);
            }
            
            m_gameObjectFactory = gameObjectFactory;
            m_behaviourInitializer = behaviourInitializer;
            m_batchSize = batchSize;
            if (batchSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch size must be greater than zero");
            }
            CreateBatch(initialSize);
        }
        
        ~GameObjectPool()
        {
            RemoveAll();
        }
        
        
        public T GetObject()
        {
            
            if(m_objectInPoolCount == 0)
            {
                CreateBatch(m_batchSize);
            }

            T result;
            lock (m_lockObject)
            {
                result = m_objects.FirstOrDefault(e => e.IsActive != true);
                TryActivateObject(result);
            }
            
            return result;
        }
        
        public T GetObject(Vector3 position, Quaternion? rotation = null)
        {
            if(m_objectInPoolCount == 0)
            {
                CreateBatch(m_batchSize);
            }

            T result;
            lock (m_lockObject)
            {
                result = m_objects.FirstOrDefault(e => e.IsActive != true);
                if (result != null)
                {
                    result.transform.position = position;
                    if (rotation != null)
                    {
                        result.transform.rotation = rotation.Value;
                    }
                    
                }

                TryActivateObject(result);
            }
            
            return result;
        }
        
        public void ReturnObject(IPoolableObject obj)
        {
            if(obj == null) throw new ArgumentNullException(nameof(obj), "Argument cannot be null.");
            if(obj is not T tObj)
            {
                throw new AggregateException($"Object is not of type {typeof(T)}.");
            }
            
            if(!TryDeactivateObject(tObj))
            {
                Debug.LogError("Object is not managed by the pool or is already inactive.");
            }
        }

        public void ReturnAllObjects()
        {
            foreach (var obj in m_objects)
            {
                if(obj.IsActive == true)
                {
                    TryDeactivateObject(obj);
                }
            }
        }

        public void Remove(IPoolableObject obj)
        {
            if(obj == null) throw new ArgumentNullException(nameof(obj), "Argument cannot be null.");
            if(obj is not T tObj)
            {
                Debug.LogError("Object is not a PoolableMonoBehaviour");
                return;
            }

            if (!TryRemoveObjectFromPool(tObj))
            {
                Debug.LogError("Object is not managed by the pool or is already inactive.");
            }
                
        }

        public void RemoveAll()
        {
            while (ObjectCount > 0)
            {
                TryRemoveObjectFromPool(m_objects.First());
            }
        }

        private void CreateObject()
        {
            GameObject go = m_gameObjectFactory != null 
                ? m_gameObjectFactory() : new GameObject($"PooledObject {ObjectCount}");

            EnsureContainer();
            
            go.transform.SetParent(m_container);
            
            T component = go.AddComponent<T>();
            component.Pool = this;
            
            m_behaviourInitializer?.Invoke(component);
            TryAddObjectToPool(component);
        }
        
        private void EnsureContainer()
        {
            if (m_container == null)
            {
                m_container = new GameObject("GameObjectPool").transform;

                if (m_dontDestroyOnLoad)
                {
                    Object.DontDestroyOnLoad(m_container.gameObject);
                }
                
            }
        }
        private void CreateBatch(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateObject();
            }
        }
        
        private bool TryAddObjectToPool(T obj)
        {
            lock (m_lockObject)
            {
                if(m_objects.Add(obj))
                {
                    obj.Deactivate();
                    m_objectInPoolCount++;
                    return true;
                }

                return false;   
            }
        }
        
        private bool TryActivateObject(T obj)
        {
            lock (m_lockObject)
            {
                if(m_objects.Contains(obj) && obj.IsActive != true)
                {
                    m_objectInPoolCount--;
                    obj.Activate();
                    return true;
                }

                return false;   
            }
        }
        
        private bool TryDeactivateObject(T obj)
        {
            lock (m_lockObject)
            {
                if(m_objects.Contains(obj) && obj.IsActive != false)
                {
                    m_objectInPoolCount++;
                    obj.Deactivate();
                    return true;
                }

                return false;
            }
        }
        
        private bool TryRemoveObjectFromPool(T obj)
        {
            lock (m_lockObject)
            {
                if(m_objects.Remove(obj))
                {
                    if(obj.IsActive == true)
                    {
                        obj.Deactivate();
                    }
                    else
                    {
                        m_objectInPoolCount--;
                    }
                    obj.Pool = null;
                    Object.Destroy(obj.gameObject);
                    return true;
                }

                return false;
            }
        }
    }
    
    public class GameObjectPool : GameObjectPool<PoolableMonoBehaviour>
    {
        public GameObjectPool(
            Func<GameObject> gameObjectFactory = null, 
            int initialSize = 10,
            int batchSize = 10, 
            Transform container = null) 
            : base(gameObjectFactory, null, initialSize, batchSize, container)
        {
        }
    }
}
