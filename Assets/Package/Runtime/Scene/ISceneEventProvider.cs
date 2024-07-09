using System;

namespace SnakeCore.Scene
{
    /// <summary>
    /// Provides scene event subscription services. <seealso cref="ISceneEventHistory"/>
    /// </summary>
    public interface ISceneEventProvider
    {
        /// <summary>
        /// Subscribe to scene events of type T. Only subscribes to type of T. Inheritance is not considered.
        /// If already subscribed, does nothing.
        /// </summary>
        /// <param name="action">Action to invoke when a scene event of type T is published.</param>
        /// <typeparam name="T">Type of scene event to subscribe to.</typeparam>
        void Subscribe<T>(Action<SceneEvent> action) where T : SceneEvent;
        
        /// <summary>
        /// Unsubscribe from scene events of type T. Only unsubscribes from type of T. Inheritance is not considered.
        /// If not subscribed, does nothing.
        /// </summary>
        /// <param name="action">Action to unsubscribe.</param>
        /// <typeparam name="T">Type of scene event to unsubscribe from.</typeparam>
        void Unsubscribe<T>(Action<SceneEvent> action) where T : SceneEvent;

        /// <summary>
        /// Subscribe to scene events of type sceneEventType. Only subscribes to type of sceneEventType.
        /// Inheritance is not considered. If already subscribed, does nothing.
        /// </summary>
        /// <param name="sceneEventType">Type of scene event to subscribe to.</param>
        /// <param name="action">Action to invoke when a scene event of type sceneEventType is published.</param>
        void Subscribe(Type sceneEventType, Action<SceneEvent> action);
        
        /// <summary>
        /// Unsubscribe from scene events of type sceneEventType. Only unsubscribes from type of sceneEventType.
        /// Inheritance is not considered.
        /// </summary>
        /// <param name="sceneEventType">Type of scene event to unsubscribe from.</param>
        /// <param name="action"> Action to unsubscribe.</param>
        void Unsubscribe(Type sceneEventType, Action<SceneEvent> action);
    }
}
