namespace TahaCore.Logging
{
    public interface ILogger
    {
        void LogError(object message);
        void LogWarning(object message);
        void LogInfo(object message);
    }
}