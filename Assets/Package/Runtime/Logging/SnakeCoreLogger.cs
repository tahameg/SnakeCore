// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using SnakeCore.DI;

namespace SnakeCore.Logging
{
    /// <summary>
    /// SnakeCoreLogger is a simple logger that uses Unity's Debug.Log, Debug.LogWarning and Debug.LogError
    /// </summary>
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(ILogger))]
    public class SnakeCoreLogger : ILogger
    {
        public void LogError(object message)
        {
            UnityEngine.Debug.LogError( $"[TahaCore Error] {message}");
        }

        public void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning($"[TahaCore Warning] {message}");
        }

        public void LogInfo(object message)
        {
            UnityEngine.Debug.Log( $"[TahaCore Info] {message}");
        }
    }
}