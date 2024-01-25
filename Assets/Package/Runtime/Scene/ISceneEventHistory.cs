namespace TahaCore.Scene
{
    /// <summary>
    /// Publishes scene events to subscribers. <seealso cref="ISceneEventProvider"/>
    /// </summary>
    public interface ISceneEventHistory 
    {
        /// <summary>
        /// Publishes a scene event to its subscribers. Only subscribers of type T will be notified.
        /// Inheritance is not considered.
        /// </summary>
        /// <param name="sceneEvent">Scene event to publish.</param>
        /// <typeparam name="T">Type of scene event to publish.</typeparam>
        void AddSceneEvent<T>(T sceneEvent) where T : SceneEvent;
        
        /// <summary>
        /// Publishes a scene event to its subscribers. Only subscribers for type of scene event will be notified.
        /// Inheritance is not considered.
        /// </summary>
        /// <param name="sceneEvent">Scene event to publish.</param>
        void AddSceneEvent(SceneEvent sceneEvent);
    }
}
