﻿using TahaCore.Runtime.DI;

namespace TahaCore.Runtime.Logging
{
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(ILogger))]
    public class TahaCoreLogger : ILogger
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