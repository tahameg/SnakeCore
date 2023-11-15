using TahaCore.Logging;

namespace TahaCore
{
    public class SimpleLogger : ILogger
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