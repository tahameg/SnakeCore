using System;
using System.Collections.Generic;
using SnakeCore.DI;
using VContainer;

namespace SnakeCore.Scene
{
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(ISceneEventHistory), typeof(ISceneEventProvider))]
    internal class SceneEventManager : ISceneEventHistory, ISceneEventProvider
    {
        private readonly Dictionary<Type, HashSet<Action<SceneEvent>>> m_subscribers = new();
        private readonly Queue<SceneEvent> m_eventQueue = new();
        private readonly int m_maxHistorySize;
        
        private const int k_maxQueueSize = 40;
        
        [Inject]
        public SceneEventManager(SceneEventSettingsConfigSection settingsConfigSection)
        {
            m_maxHistorySize = settingsConfigSection.MaxHistorySize == default ? k_maxQueueSize : settingsConfigSection.MaxHistorySize;
        }
        
        public void AddSceneEvent<T>(T sceneEvent) where T : SceneEvent
        {
            AddToHistory(sceneEvent);
            Type sceneEventType = typeof(T);
            AddSceneEventByType(sceneEvent, sceneEventType);
        }

        public void AddSceneEvent(SceneEvent sceneEvent)
        {
            Type sceneEventType = sceneEvent.GetType();
            AddSceneEventByType(sceneEvent, sceneEventType);
        }

        public void Subscribe<T>(Action<SceneEvent> action) where T : SceneEvent
        {
            Type sceneEventType = typeof(T);
            Subscribe(sceneEventType, action);
        }

        public void Unsubscribe<T>(Action<SceneEvent> action) where T : SceneEvent
        {
            Type sceneEventType = typeof(T);
            Unsubscribe(sceneEventType, action);
        }
        
        public void Subscribe(Type sceneEventType, Action<SceneEvent> action)
        {
            if(m_subscribers.TryGetValue(sceneEventType, out var subscribers))
            {
                if (subscribers.Contains(action))
                {
                    SnakeCoreApplicationRuntime.LogWarning($"Action {action} already subscribed to {sceneEventType}");
                    return;
                }
                subscribers.Add(action);
                return;
            }
            subscribers = new HashSet<Action<SceneEvent>> { action };
            m_subscribers.Add(sceneEventType, subscribers);
        }

        public void Unsubscribe(Type sceneEventType, Action<SceneEvent> action)
        {
            if(!m_subscribers.TryGetValue(sceneEventType, out var subscribers))
            {
                return;
            }

            if (!subscribers.Contains(action))
            {
                return;
            }
            
            subscribers.Remove(action);
        }

        private void AddToHistory(SceneEvent sceneEvent)
        {
            if (m_eventQueue.Count >= m_maxHistorySize)
            {
                m_eventQueue.Dequeue();
            }

            m_eventQueue.Enqueue(sceneEvent);
        }
        
        private void AddSceneEventByType(SceneEvent sceneEvent, Type sceneEventType)
        {
            AddToHistory(sceneEvent);
            if (m_subscribers.TryGetValue(sceneEventType, out var subscriber))
            {
                foreach (Action<SceneEvent> action in subscriber)
                {
                    action.Invoke(sceneEvent);
                }
            }
        }
    }
}
